using System.Collections.Generic;
using System.IO;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class ProjectInformation
	{
		public ProjectInformation(
			DirectoryInfo repositoryRootDirectory,
			DirectoryInfo sourceCodeRootDirectory,
			IEnumerable<Module> modules
		)
		{
			RepositoryRootDirectory = repositoryRootDirectory;
			Modules = modules;
			SourceCodeRootDirectory = sourceCodeRootDirectory;
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IEnumerable<Module> Modules { get; }
	}
}