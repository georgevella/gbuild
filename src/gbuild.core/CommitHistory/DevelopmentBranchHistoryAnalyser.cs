using System.Collections.Generic;
using System.Linq;
using GBuild.Models;
using LibGit2Sharp;
using Serilog;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	class DevelopmentBranchHistoryAnalyser : IBranchHistoryAnalyser
	{
		private readonly IRepository _gitRepository;

		public DevelopmentBranchHistoryAnalyser(
			IRepository gitRepository
			)
		{
			_gitRepository = gitRepository;
		}
		public string ParentBranch { get; set; }
		public IList<Commit> GetNewCommits()
		{
			var currentBranch = _gitRepository.Branches.First(b => b.IsCurrentRepositoryHead);
			var parentBranch = _gitRepository.Branches.First(b => b.CanonicalName == ParentBranch);

			Log.Debug("Commit analysis running between current branch [{currentbranch}] and [{parentbranch}:{parentcommit}]",
					  currentBranch.Tip.Sha,
					  ParentBranch,
					  parentBranch.Tip.Sha);

			var filter = new CommitFilter
			{
				ExcludeReachableFrom = parentBranch,
				IncludeReachableFrom = currentBranch,
				SortBy = CommitSortStrategies.Time
			};

			return _gitRepository.Commits.QueryBy(filter).Select(BuildCommitEntry).ToList();
		}

		private Commit BuildCommitEntry(LibGit2Sharp.Commit arg)
		{
			// TODO: store merge commit parental history
			var treeChanges = new List<TreeEntryChanges>();

			foreach (var parent in arg.Parents)
			{
				treeChanges.AddRange(
					_gitRepository.Diff.Compare<TreeChanges>(parent.Tree, arg.Tree)
				);
			}

			var changedFiles = treeChanges.Select(e => new ChangedFile(e.Path)).ToList();

			return new Commit(arg.Id.Sha, arg.Committer.Name, arg.Message, changedFiles);
		}
	}
}