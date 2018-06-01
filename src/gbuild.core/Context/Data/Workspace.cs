﻿using System.Collections.Generic;
using System.IO;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class Workspace
	{
		public Workspace(
			DirectoryInfo repositoryRootDirectory,
			DirectoryInfo sourceCodeRootDirectory,
			IEnumerable<Project> modules
		)
		{
			RepositoryRootDirectory = repositoryRootDirectory;
			Modules = modules;
			SourceCodeRootDirectory = sourceCodeRootDirectory;
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IEnumerable<Project> Modules { get; }
	}
}