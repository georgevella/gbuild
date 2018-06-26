using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace gbuild.commitanalysis.git
{
	public class GitRepositoryHelpers : IGitRepositoryHelpers
	{
		private readonly IRepository _repository;

		public GitRepositoryHelpers(IRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<TreeEntryChanges> CompareTrees(Tree oldTree, Tree newTree)
		{
			return _repository.Diff.Compare<TreeChanges>(oldTree, newTree);
		}

		public IEnumerable<PatchEntryChanges> ComparePatch(Tree oldTree, Tree newTree)
		{
			return _repository.Diff.Compare<Patch>(oldTree, newTree);
		}
	}
}