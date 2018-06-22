using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBuild.Configuration.Models;

namespace GBuild.Models
{
	public class Workspace
	{
		public Workspace(
			DirectoryInfo repositoryRootDirectory,
			DirectoryInfo sourceCodeRootDirectory,
			IEnumerable<Project> projects,
			IEnumerable<Release> releases,
			IBranchVersioningStrategyModel branchVersioningStrategy
		)
		{
			RepositoryRootDirectory = repositoryRootDirectory;
			Projects = projects.ToList();
			SourceCodeRootDirectory = sourceCodeRootDirectory;
			BranchVersioningStrategy = branchVersioningStrategy;
			Releases = releases.ToList();
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IBranchVersioningStrategyModel BranchVersioningStrategy { get; }

		public IReadOnlyList<Project> Projects { get; }

		public IReadOnlyList<Release> Releases { get; }
	}
}