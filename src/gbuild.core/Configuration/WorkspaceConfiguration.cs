using System.Collections.Generic;
using GBuild.Configuration.Models;

namespace GBuild.Configuration
{
	public class WorkspaceConfiguration : IWorkspaceConfiguration
	{
		public IEnumerable<IBranchVersioningStrategyModel> BranchVersioningStrategies { get; }

		public WorkspaceConfiguration(IConfigurationFile configurationFile)
		{
			BranchVersioningStrategies = configurationFile.Branches;

			StartingVersion = SemanticVersion.Parse(configurationFile.StartingVersion);

			SourceCodeRoot = configurationFile.SourceCodeRoot;
		}

		public string SourceCodeRoot { get; }

		public SemanticVersion StartingVersion { get; }
	}
}