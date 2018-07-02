using System.Collections.Generic;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.Configuration
{
	public class WorkspaceConfiguration : IWorkspaceConfiguration
	{
		public WorkspaceConfiguration(IConfigurationFileLoader configurationFileLoader)
		{
			var configuration = configurationFileLoader.Load();

			KnownBranches = configuration.Branches.KnownBranches.Select(
				b => new KnownBranch(
					b.Name,
					string.IsNullOrWhiteSpace(b.Pattern) ? b.Name : b.Pattern,
					b.Type,
					new BranchVersioningStrategy()
					{
						Metadata = b.Versioning.Metadata,
						Name = b.Name,
						Tag = b.Versioning.Tag,
						ParentBranch = b.Versioning.ParentBranch,
						Increment = b.Versioning.Increment
					}
				)
			).ToList();

			BranchVersioningStrategies = KnownBranches.Select(knownBranch => knownBranch.VersioningStrategy).ToList();

			StartingVersion = SemanticVersion.Parse(configuration.StartingVersion);

			SourceCodeRoot = configuration.Sources;
		}
		public IEnumerable<IBranchVersioningStrategy> BranchVersioningStrategies { get; }
		public IEnumerable<IKnownBranch> KnownBranches { get; }
		public string SourceCodeRoot { get; }

		public SemanticVersion StartingVersion { get; }
	}
}