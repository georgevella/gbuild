using System.Linq;
using GBuild.Configuration;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Generator
{
	/// <summary>
	///     Version number generator for a branch that is the 'development' counter part of an other branch designated as the
	///     'production' branch.
	/// </summary>
	public class IndependentVersionNumberGenerator : IVersionNumberGenerator
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly CommitHistoryAnalysis _commitAnalysis;
		private readonly Workspace _workspace;

		public IndependentVersionNumberGenerator(
			IWorkspaceConfiguration workspaceConfiguration,
			IContextData<CommitHistoryAnalysis> commitAnalysis,
			IContextData<Workspace> workspace)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_commitAnalysis = commitAnalysis.Data;
			_workspace = workspace.Data;
		}

		public SemanticVersion GetVersion(Project project)
		{
			var branchVersioningStrategyModel = _workspace.BranchVersioningStrategy;

			var baseVersion = _workspaceConfiguration.StartingVersion;
			int projectCommitCount = 0;

			// TODO: check if we have any releases from the commit history analyser
			if (_workspace.Releases.Any())
			{
				var release = _workspace.Releases.First();

				baseVersion = _commitAnalysis.HasBreakingChanges 
					? release.VersionNumbers[project].IncrementMajor() 
					: release.VersionNumbers[project].IncrementMinor();				
			}

			if (_commitAnalysis.ChangedProjects.TryGetValue(project, out var commits))
			{
				projectCommitCount = commits.Count;
			}

			return SemanticVersion.Create(
				major: baseVersion.Major,
				minor: baseVersion.Minor,
				patch: baseVersion.Patch,
				prereleseTag: $"{branchVersioningStrategyModel.Tag}-{projectCommitCount}",
				metadata: branchVersioningStrategyModel.Metadata
			);
		}
	}
}