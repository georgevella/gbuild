using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GBuild.CommitHistory;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Context.Attributes;
using GBuild.Generator;

namespace GBuild.Models
{
	/// <summary>
	///		Contextual data about the current state of the workspace.
	/// </summary>
	[DependsOnContextData(typeof(Process))]
	public class Workspace : IContextEntity
	{
		public Workspace(
			DirectoryInfo repositoryRootDirectory,
			DirectoryInfo sourceCodeRootDirectory,
			IEnumerable<Project> projects,
			IKnownBranch branchModel
		)
		{
			if (projects == null)
			{
				throw new ArgumentNullException(nameof(projects));
			}

			RepositoryRootDirectory = repositoryRootDirectory ?? throw new ArgumentNullException(nameof(repositoryRootDirectory));
			Projects = projects.ToList();
			SourceCodeRootDirectory = sourceCodeRootDirectory ?? throw new ArgumentNullException(nameof(sourceCodeRootDirectory));
			BranchModel = branchModel;
		}

		public DirectoryInfo RepositoryRootDirectory { get; }

		public DirectoryInfo SourceCodeRootDirectory { get; }
		public IKnownBranch BranchModel { get; }

		public IReadOnlyList<Project> Projects { get; }		
	}
}