using System.Collections.Generic;
using GBuild.Core.Configuration;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class BranchInformation
	{
		public BranchInformation(
			Branch currentBranch,
			IBranchVersioningStrategy versioningStrategy
		)
		{
			CurrentBranch = currentBranch;
			VersioningStrategy = versioningStrategy;
		}

		public Branch CurrentBranch { get; }

		public IBranchVersioningStrategy VersioningStrategy { get; }
	}
}