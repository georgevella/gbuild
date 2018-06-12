using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;

namespace GBuild.Core.Generator
{
	/// <summary>
	///     Version number generator for a branch that is the 'development' counter part of an other branch designated as the
	///     'production' branch.
	/// </summary>
	public class IndependentVersionNumberGenerator : IVersionNumberGenerator
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly IContextData<CommitAnalysisResult> _commitAnalysis;
		private readonly IContextData<Workspace> _workspace;

		public IndependentVersionNumberGenerator(
			IWorkspaceConfiguration workspaceConfiguration,
			IContextData<CommitAnalysisResult> commitAnalysis,
			IContextData<Workspace> workspace)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_commitAnalysis = commitAnalysis;
			_workspace = workspace;
		}

		public SemanticVersion GetVersion(Project project)
		{
			var branchVersioningStrategyModel = _commitAnalysis.Data.VersioningStrategyModel;
			var startingVersion = _workspaceConfiguration.StartingVersion;			

			return SemanticVersion.Create(
				major: startingVersion.Major,
				minor: startingVersion.Minor,
				patch: startingVersion.Patch,
				prereleseTag: $"{branchVersioningStrategyModel.Tag}-{_commitAnalysis.Data.Commits.Count}",
				metadata: branchVersioningStrategyModel.Metadata
			);
		}
	}
}