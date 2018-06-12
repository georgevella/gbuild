﻿using System.Collections.Generic;
using LibGit2Sharp;
using Branch = gbuild.commitanalysis.git.Models.Branch;

namespace gbuild.commitanalysis.git
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