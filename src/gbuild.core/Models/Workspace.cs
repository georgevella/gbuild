using System.Collections.Generic;
using System.IO;

namespace GBuild.Models
{
	public class Workspace
	{
		public Workspace(
			DirectoryInfo repositoryRootDirectory,
			DirectoryInfo sourceCodeRootDirectory,
			IEnumerable<Project> projects
		)
		{
			RepositoryRootDirectory = repositoryRootDirectory;
			Projects = projects;
			SourceCodeRootDirectory = sourceCodeRootDirectory;
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IEnumerable<Project> Projects { get; }
	}
}