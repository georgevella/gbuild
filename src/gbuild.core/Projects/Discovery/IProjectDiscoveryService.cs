using System.Collections.Generic;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Projects.Discovery
{
	public interface IProjectDiscoveryService
	{
		IEnumerable<Project> GetProjects();
	}
}