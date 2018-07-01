using System.Collections.Generic;
using System.Linq;
using GBuild.Models;
using GBuild.Workspaces;

namespace GBuild.Projects.Discovery
{
	public class ProjectDiscoveryService : IProjectDiscoveryService
	{
		private readonly IWorkspaceSourceCodeDirectoryProvider _workspaceSourceCodeDirectoryProvider;
		private readonly List<IProjectEnumerationService> _projectEnumerationServices;

		public ProjectDiscoveryService(
			IWorkspaceSourceCodeDirectoryProvider workspaceSourceCodeDirectoryProvider,
			IEnumerable<IProjectEnumerationService> projectEnumerationServices
		)
		{
			_workspaceSourceCodeDirectoryProvider = workspaceSourceCodeDirectoryProvider;
			_projectEnumerationServices = projectEnumerationServices.ToList();
		}

		/// <inheritdoc />
		public IEnumerable<Project> GetProjects()
		{
			var sourceCodeRootDirectory = _workspaceSourceCodeDirectoryProvider.GetSourceCodeDirectory();

			return _projectEnumerationServices.SelectMany( s => s.GetProjects(sourceCodeRootDirectory)).ToList();
		}
	}
}