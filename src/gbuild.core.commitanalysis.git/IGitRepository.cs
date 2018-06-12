using System.Collections.Generic;
using LibGit2Sharp;
using Branch = GBuild.Core.CommitAnalysis.Git.Models.Branch;

namespace GBuild.Core.CommitAnalysis
{
	public interface IGitRepository
	{
		IEnumerable<Branch> Branches { get; }
		IQueryableCommitLog Commits { get; }

		IEnumerable<TreeEntryChanges> CompareTrees(
			Tree oldTree,
			Tree newTree
		);

		IEnumerable<PatchEntryChanges> ComparePatch(Tree oldTree, Tree newTree);
	}
}