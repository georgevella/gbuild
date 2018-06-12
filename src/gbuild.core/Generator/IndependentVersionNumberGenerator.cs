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
			var branchVersioningStrategyModel = _commitAnalysis.VersioningStrategyModel;

			var startingVersion = _workspaceConfiguration.StartingVersion;			

			// TODO: check if we have any releases from the commit history analyser
			int projectCommitCount = 0;
			if (_commitAnalysis.ChangedProjects.TryGetValue(project, out var commits))
			{
				projectCommitCount = commits.Count;
			}
			
			return SemanticVersion.Create(
				major: startingVersion.Major,
				minor: startingVersion.Minor,
				patch: startingVersion.Patch,
				prereleseTag: $"{branchVersioningStrategyModel.Tag}-{projectCommitCount}",
				metadata: branchVersioningStrategyModel.Metadata
			);
		}
	}
}