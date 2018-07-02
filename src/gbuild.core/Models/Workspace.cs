using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			IBranchVersioningStrategy branchVersioningStrategy,
			IDictionary<string, string> variables = null
		)
		{
			if (projects == null)
			{
				throw new ArgumentNullException(nameof(projects));
			}

			RepositoryRootDirectory = repositoryRootDirectory ?? throw new ArgumentNullException(nameof(repositoryRootDirectory));
			Projects = projects.ToList();
			SourceCodeRootDirectory = sourceCodeRootDirectory ?? throw new ArgumentNullException(nameof(sourceCodeRootDirectory));
			BranchVersioningStrategy = branchVersioningStrategy ?? throw new ArgumentNullException(nameof(branchVersioningStrategy));			
			Variables = new ReadOnlyDictionary<string, string>(variables ?? new Dictionary<string, string>());
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IBranchVersioningStrategy BranchVersioningStrategy { get; }

		public IReadOnlyList<Project> Projects { get; }		

		public IReadOnlyDictionary<string, string> Variables { get; }
	}
}