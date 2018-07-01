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
			IEnumerable<Release> releases,
			IBranchVersioningStrategyModel branchVersioningStrategy,
			WorkspaceVersionInfo projectLatestVersion,
			IDictionary<string, string> variables = null
		)
		{
			if (projects == null)
			{
				throw new ArgumentNullException(nameof(projects));
			}

			if (releases == null)
			{
				throw new ArgumentNullException(nameof(releases));
			}

			RepositoryRootDirectory = repositoryRootDirectory ?? throw new ArgumentNullException(nameof(repositoryRootDirectory));
			Projects = projects.ToList();
			SourceCodeRootDirectory = sourceCodeRootDirectory ?? throw new ArgumentNullException(nameof(sourceCodeRootDirectory));
			BranchVersioningStrategy = branchVersioningStrategy ?? throw new ArgumentNullException(nameof(branchVersioningStrategy));
			ProjectLatestVersion = projectLatestVersion ?? throw new ArgumentNullException(nameof(projectLatestVersion));
			Releases = releases.ToList();
			Variables = new ReadOnlyDictionary<string, string>(variables ?? new Dictionary<string, string>());
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }

		public IBranchVersioningStrategyModel BranchVersioningStrategy { get; }

		public IReadOnlyList<Project> Projects { get; }

		public IReadOnlyList<Release> Releases { get; }

		public WorkspaceVersionInfo ProjectLatestVersion { get; }

		public IReadOnlyDictionary<string, string> Variables { get; }
	}
}