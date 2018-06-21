using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GBuild.Models
{
	public class Workspace
	{
		public Workspace(
			DirectoryInfo repositoryRootDirectory,
			DirectoryInfo sourceCodeRootDirectory,
			IEnumerable<Project> projects,
			IEnumerable<Release> releases
		)
		{
			RepositoryRootDirectory = repositoryRootDirectory;
			Projects = projects.ToList();
			SourceCodeRootDirectory = sourceCodeRootDirectory;
			Releases = releases.ToList();
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IReadOnlyList<Project> Projects { get; }

		public IReadOnlyList<Release> Releases { get; }
	}
}