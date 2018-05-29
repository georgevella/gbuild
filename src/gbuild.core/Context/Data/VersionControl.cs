using System.Collections.Generic;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class VersionControl
	{
		public VersionControl(
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