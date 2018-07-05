using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using GBuild.ReleaseHistory;
using GBuild.Variables;
using LibGit2Sharp;
using Serilog;
using YamlDotNet.Serialization.NodeTypeResolvers;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	[SupportedBranchType(BranchType.Development)]
	[SupportedBranchType(BranchType.Feature)]
	class DevelopmentBranchHistoryAnalyser : IBranchHistoryAnalyser
	{
		private readonly IRepository _repository;
		private readonly IContextData<ActiveReleases> _activeReleasesData;

		public DevelopmentBranchHistoryAnalyser(
			IRepository repository,
			IContextData<ActiveReleases> activeReleasesData
			)
		{
			_repository = repository;
			_activeReleasesData = activeReleasesData;
		}
		public IEnumerable<Commit> GetCommitsTowardsTarget(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		)
		{			
			var thisBranch = _repository.Branches.First(b => b.CanonicalName == branchName);
			var mergeTargetBranch = _repository.Branches.First(b => b.CanonicalName == branchAnalysisSettings.MergeTarget);

			Log.Debug("Fetching commits between current branch [{currentbranch}] and merge target [{mergetarget}:{targetcommit}]",
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
			var activeReleases = _activeReleasesData.Data;

			var thisBranch = _repository.Branches.First(b => b.CanonicalName == branchName);
			var parentBranch = _repository.Branches.First(b => b.CanonicalName == branchAnalysisSettings.ParentBranch);

			Log.Debug("Fetching commits between current branch [{currentbranch}] and parent [{parentbranch}:{parentcommit}]",
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