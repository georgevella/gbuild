using System.Collections.Generic;
using System.IO;
using GBuild.Models;

namespace GBuild.Projects.Discovery
{
	public interface IProjectEnumerationService
	{
		IEnumerable<Project> GetProjects(DirectoryInfo sourceCodeRootDirectory);
	}
}