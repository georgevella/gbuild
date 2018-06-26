using System.Collections.Generic;
using LibGit2Sharp;

namespace GBuild.Vcs
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