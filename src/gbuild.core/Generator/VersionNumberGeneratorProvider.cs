using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Generator
{
	internal class VersionNumberGeneratorProvider : IVersionNumberGeneratorProvider
	{
		private readonly IContextData<Workspace> _workspaceContextData;
		private readonly IBranchVersioningStrategy _branchVersioningStrategy;
		private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

		public VersionNumberGeneratorProvider(
			IEnumerable<IVersionNumberGenerator> versionNumberGenerators,
			IContextData<Workspace> workspaceContextData,
			IBranchVersioningStrategy branchVersioningStrategy
		)
		{
			_versionNumberGenerators = versionNumberGenerators;
			_workspaceContextData = workspaceContextData;
			_branchVersioningStrategy = branchVersioningStrategy;
		}

		public WorkspaceVersionInfo GetVersion(CommitHistoryAnalysis commitHistoryAnalysis)
		{
			// TODO: implement mapping between branch strategy and version number generator
			var versionNumberGenerator = _versionNumberGenerators.First();

			var workspaceVersionInfo = new Dictionary<Project, SemanticVersion>();

			foreach (var project in _workspaceContextData.Data.Projects)
			{
				var projectVersion = versionNumberGenerator.GetVersion(commitHistoryAnalysis, _branchVersioningStrategy, _workspaceContextData.Data.BranchModel.VersioningSettings, project);
				workspaceVersionInfo[project] = projectVersion;
			}

			return new WorkspaceVersionInfo(workspaceVersionInfo);
		}

		public WorkspaceVersionInfo GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis,
			IBranchVersioningSettings branchVersioningSettings
		)
		{
			var versionNumberGenerator = _versionNumberGenerators.First();

			var workspaceVersionInfo = new Dictionary<Project, SemanticVersion>();

			foreach (var project in _workspaceContextData.Data.Projects)
			{
				var projectVersion = versionNumberGenerator.GetVersion(commitHistoryAnalysis, _branchVersioningStrategy, branchVersioningSettings, project);
				workspaceVersionInfo[project] = projectVersion;
			}

			return new WorkspaceVersionInfo(workspaceVersionInfo);
		}
	}
}