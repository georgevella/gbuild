using System.Collections.Generic;
using LibGit2Sharp;

namespace gbuild.commitanalysis.git
{
	public interface IGitRepositoryHelpers
	{
		IEnumerable<TreeEntryChanges> CompareTrees(
			Tree oldTree,
			Tree newTree
		);

		IEnumerable<PatchEntryChanges> ComparePatch(Tree oldTree, Tree newTree);
	}
}