using System.Linq;
using GBuild.CommitHistory;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	[SupportedBranchType(BranchType.Development)]
	[SupportedBranchType(BranchType.Feature)]
	public class DevelopmentBranchVersioningStrategy : IBranchVersioningStrategy
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly IContextData<ActiveReleases> _activeReleases;
		private readonly IContextData<PastReleases> _pastReleases;

		public DevelopmentBranchVersioningStrategy(
			IWorkspaceConfiguration workspaceConfiguration,
			IContextData<ActiveReleases> activeReleases,
			IContextData<PastReleases> pastReleases
			)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_activeReleases = activeReleases;
			_pastReleases = pastReleases;
		}
		public SemanticVersion Generate(
			IBranchVersioningSettings branchVersioningSettings,
			SemanticVersion baseVersion,
			Project project,
			IVariableStore variableStore
		)
		{
			//baseVersion = _workspaceConfiguration.StartingVersion;

			var activeReleases = _activeReleases.Data;
			if (activeReleases.Any())
			{
				// TODO: handle multiple releases
				baseVersion = activeReleases.First().VersionNumbers[project];

				return baseVersion.IncrementMinor();
			}

			return baseVersion;
		}
	}
}