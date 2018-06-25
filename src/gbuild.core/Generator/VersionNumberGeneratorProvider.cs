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
		private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

		public VersionNumberGeneratorProvider(
			IEnumerable<IVersionNumberGenerator> versionNumberGenerators,
			IContextData<Workspace> workspaceContextData
		)
		{
			_versionNumberGenerators = versionNumberGenerators;
			_workspaceContextData = workspaceContextData;
		}

		public WorkspaceVersionInfo GetVersion()
		{
			// TODO: implement mapping between branch strategy and version number generator
			var versionNumberGenerator = _versionNumberGenerators.First();

			var workspaceVersionInfo = new Dictionary<Project, SemanticVersion>();

			foreach (var project in _workspaceContextData.Data.Projects)
			{
				var projectVersion = versionNumberGenerator.GetVersion(project);
				workspaceVersionInfo[project] = projectVersion;
			}

			return new WorkspaceVersionInfo(workspaceVersionInfo);
		}
	}
}