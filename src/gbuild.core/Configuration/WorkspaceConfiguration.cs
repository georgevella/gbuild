using System.Collections.Generic;
using GBuild.Core.Configuration.Models;

namespace GBuild.Core.Configuration
{
	public class WorkspaceConfiguration : IWorkspaceConfiguration
	{
		public IEnumerable<IBranchVersioningStrategyModel> BranchVersioningStrategies { get; }

		public WorkspaceConfiguration(IConfigurationFile configurationFile)
		{
			BranchVersioningStrategies = configurationFile.Branches;

			StartingVersion = SemanticVersion.Parse(configurationFile.StartingVersion);
		}

		public SemanticVersion StartingVersion { get; }
	}
}