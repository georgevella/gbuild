using System.Linq;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	/// <summary>
	///     Version number generator for a branch that is the 'development' counter part of an other branch designated as the
	///     'production' branch.
	/// </summary>
	public class IndependentVersionNumberGenerator : IVersionNumberGenerator
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly IBranchVersioningStrategyProvider _branchVersioningStrategyProvider;
		private readonly IVariableRenderer _variableRenderer;
		private readonly PastReleases _pastReleases;

		public IndependentVersionNumberGenerator(
			IWorkspaceConfiguration workspaceConfiguration,
			IContextData<PastReleases> pastReleases,
			IBranchVersioningStrategyProvider branchVersioningStrategyProvider,
			IVariableRenderer variableRenderer
			)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_branchVersioningStrategyProvider = branchVersioningStrategyProvider;
			_variableRenderer = variableRenderer;
			_pastReleases = pastReleases.Data;
		}

		public SemanticVersion GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis,
			Project project,
			IVariableStore variableStore)
		{ 	

			var baseVersion = _workspaceConfiguration.StartingVersion;

			// TODO: check for any release branches, that are in progress, and run a commit analysis on latest release branch and version generation logic
			// TODO: implement base version number retireval in branch versioning strategy!!!!
			
			if (_pastReleases.Any())
			{
				var release = _pastReleases.First();

				baseVersion = release.VersionNumbers[project];

				if (commitHistoryAnalysis.ChangedProjects.TryGetValue(project, out var changedProject))
				{
					if (changedProject.HasBreakingChanges)
					{
						// TODO: make this configurable from branching strategy
						baseVersion = baseVersion.IncrementMajor();
					}
					else
					{
						baseVersion = baseVersion.IncrementMinor();
					}
				}
				
				// we don't touch the version if there are no changes for this project, we simply point to the latest release.
			}

			var branchVersioningStrategy =
				_branchVersioningStrategyProvider.GetVersioningStrategy(commitHistoryAnalysis.BranchName);

			var knownBranch = _workspaceConfiguration.KnownBranches.First(x => x.IsMatch(commitHistoryAnalysis.BranchName));

			baseVersion = branchVersioningStrategy.Generate(knownBranch.VersioningSettings, baseVersion, project, variableStore);

			return SemanticVersion.Create(
				major: baseVersion.Major,
				minor: baseVersion.Minor,
				patch: baseVersion.Patch,
				prereleseTag: _variableRenderer.Render(knownBranch.VersioningSettings.Tag, project, variableStore),
				metadata: _variableRenderer.Render(knownBranch.VersioningSettings.Metadata, project, variableStore)
			);
		}
	}
}