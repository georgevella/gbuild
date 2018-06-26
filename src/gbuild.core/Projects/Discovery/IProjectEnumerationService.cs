using System.Collections.Generic;
using GBuild.Models;

namespace GBuild.Projects.Discovery
{
	public interface IProjectEnumerationService
	{
		IEnumerable<Project> GetProjects();
	}
}