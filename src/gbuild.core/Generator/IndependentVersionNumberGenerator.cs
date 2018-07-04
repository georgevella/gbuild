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
		private readonly PastReleases _pastReleases;

		public IndependentVersionNumberGenerator(
			IWorkspaceConfiguration workspaceConfiguration,
			IContextData<PastReleases> pastReleases
			)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_pastReleases = pastReleases.Data;
		}

		public SemanticVersion GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis,
			IBranchVersioningStrategy branchVersioningStrategy,
			IBranchVersioningSettings branchVersioningSettings, 
			Project project,
			IVariableStore variableStore)
		{ 	

			var baseVersion = _workspaceConfiguration.StartingVersion;

			// TODO: check for any release branches, that are in progress, and run a commit analysis on latest release branch and version generation logic
			// TODO: implement base version number retireval in branch versioning strategy!!!!
			
			if (_pastReleases.Any())
			{
				var release = _pastReleases.First();

				if (commitHistoryAnalysis.ChangedProjects.TryGetValue(project, out var changedProject))
				{
					if (changedProject.HasBreakingChanges)
					{
						// TODO: make this configurable from branching strategy
						baseVersion = release.VersionNumbers[project].IncrementMajor();
					}
					else
					{
						baseVersion = release.VersionNumbers[project].IncrementMinor();
					}
				}
				
				// we don't touch the version if there are no changes for this project, we simply point to the latest release.
			}

			return branchVersioningStrategy.Generate(branchVersioningSettings, baseVersion, project, variableStore);
		}
	}
}