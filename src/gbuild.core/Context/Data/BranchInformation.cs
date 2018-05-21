using System.Collections.Generic;
using GBuild.Core.Configuration;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class BranchInformation
	{
		public BranchInformation(
			Branch currentBranch,
			IBranchVersioningStrategyModel versioningStrategyModel
		)
		{
			CurrentBranch = currentBranch;
			VersioningStrategyModel = versioningStrategyModel;
		}

		public Branch CurrentBranch { get; }

		public IBranchVersioningStrategyModel VersioningStrategyModel { get; }
	}
}