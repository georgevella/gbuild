using System.Collections.Generic;
using GBuild.Core.Configuration;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class BranchInformation
	{
		public BranchInformation(
			Branch currentBranch,
			IEnumerable<Branch> allBranches,
			BranchVersioningStrategy versioningStrategy
		)
		{
			CurrentBranch = currentBranch;
			Branches = allBranches;
			VersioningStrategy = versioningStrategy;
		}

		public Branch CurrentBranch { get; }

		public IEnumerable<Branch> Branches { get; }

		public BranchVersioningStrategy VersioningStrategy { get; }
	}
}