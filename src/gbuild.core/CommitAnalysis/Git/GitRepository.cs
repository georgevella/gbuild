using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Branch = GBuild.Core.CommitAnalysis.Git.Branch;

namespace GBuild.Core.CommitAnalysis
{
	class GitRepository : IGitRepository
	{
		private readonly IRepository _repository;

		public GitRepository(IRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<Branch> Branches => _repository.Branches.Cast<Branch>().ToList();
		public IQueryableCommitLog Commits => _repository.Commits;

		public IEnumerable<TreeEntryChanges> CompareTrees(Tree oldTree, Tree newTree)
		{
			return _repository.Diff.Compare<TreeChanges>(oldTree, newTree);
		}
	}
}