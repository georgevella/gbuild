using System.Collections.Generic;
using LibGit2Sharp;
using Branch = GBuild.Core.CommitAnalysis.Git.Branch;

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


	}
}