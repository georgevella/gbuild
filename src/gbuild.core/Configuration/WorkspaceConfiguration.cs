using System.Collections.Generic;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.Configuration
{
	public class WorkspaceConfiguration : IWorkspaceConfiguration
	{
		public WorkspaceConfiguration(
			IConfigurationFileLoader configurationFileLoader
		)
		{
			var configuration = configurationFileLoader.Load();

			KnownBranches = configuration.Branches.KnownBranches.Select(
				b => new KnownBranch(
					b.Name,
					string.IsNullOrWhiteSpace(b.Pattern) ? b.Name : b.Pattern,
					b.Type,
					new BranchVersioningSettings()
					{
						Metadata = b.Versioning.Metadata,
						Tag = b.Versioning.Tag,
						Increment = b.Versioning.Increment
					},
					new BranchAnalysisSettings()
					{
						ParentBranch = b.Analysis.ParentBranch,
						MergeTarget = string.IsNullOrWhiteSpace(b.Analysis.MergeTarget) ? b.Analysis.ParentBranch : b.Analysis.MergeTarget
					}
				)
			).ToList();

			StartingVersion = SemanticVersion.Parse(configuration.StartingVersion);

			SourceCodeRoot = configuration.Sources;
		}

		public IEnumerable<IKnownBranch> KnownBranches { get; }
		public string SourceCodeRoot { get; }

		public SemanticVersion StartingVersion { get; }
	}
}