using System.Collections.Generic;
using System.IO;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class RepositoryInformation
	{
		public RepositoryInformation(
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