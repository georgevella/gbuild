using System.Collections.Generic;
using System.Linq;
using GBuild.Configuration.Models;
using LibGit2Sharp;
using Serilog;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	[SupportedBranchType(BranchType.Release)]
	class ReleaseBranchHistoryAnalyser : IBranchHistoryAnalyser
	{
		private readonly IRepository _repository;

		public ReleaseBranchHistoryAnalyser(
			IRepository repository
		)
		{
			_repository = repository;
		}

		public IEnumerable<Commit> GetCommitsTowardsTarget(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		)
		{
			// TODO: verify if current branch was not merged into target branch, and what to do if so

			var thisBranch = _repository.Branches.First(b => b.CanonicalName == branchName);
			var mergeTargetBranch = _repository.Branches.First(b => b.CanonicalName == branchAnalysisSettings.MergeTarget);

			Log.Debug("Commit analysis running between current branch [{currentbranch}] and [{parentbranch}:{parentcommit}]",
					  thisBranch.Tip.Sha,
					  branchAnalysisSettings.MergeTarget,
					  mergeTargetBranch.Tip.Sha);

			var filter = new CommitFilter
			{
				ExcludeReachableFrom = mergeTargetBranch,
				IncludeReachableFrom = thisBranch,
				SortBy = CommitSortStrategies.Time
			};

			return _repository.Commits.QueryBy(filter).Select(commit => _repository.BuildCommitEntry(commit)).ToList();
		}

		public IEnumerable<Commit> GetCommitsAheadOfParent(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		)
		{
			var thisBranch = _repository.Branches.First(b => b.CanonicalName == branchName);
			var parentBranch = _repository.Branches.First(b => b.CanonicalName == branchAnalysisSettings.ParentBranch);

			Log.Debug("Fetching commits between current branch [{currentbranch}] and [{parentbranch}:{parentcommit}]",
					  thisBranch.Tip.Sha,
					  branchAnalysisSettings.ParentBranch,
					  parentBranch.Tip.Sha);

			var filter = new CommitFilter
			{
				ExcludeReachableFrom = parentBranch,
				IncludeReachableFrom = thisBranch,
				SortBy = CommitSortStrategies.Time
			};

			return _repository.Commits.QueryBy(filter).Select(commit => _repository.BuildCommitEntry(commit)).ToList();
		}		
	}
}