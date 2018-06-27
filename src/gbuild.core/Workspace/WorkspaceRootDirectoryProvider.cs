using System;
using System.IO;

namespace GBuild.Workspace
{
	class WorkspaceRootDirectoryProvider : IWorkspaceRootDirectoryProvider
	{
		public DirectoryInfo GetWorkspaceRootDirectory()
		{
			var workspaceRootDirectory = new DirectoryInfo(Environment.CurrentDirectory);
			if (!workspaceRootDirectory.Exists)
			{
				throw new InvalidOperationException("Cannot find git repository root.");
			}

			do
			{
				var isWorkspaceRootDirectory = File.Exists(Path.Combine(workspaceRootDirectory.FullName, ".git"))
				                               || File.Exists(Path.Combine(workspaceRootDirectory.FullName, "build.yaml"))
				                               || Directory.Exists(Path.Combine(workspaceRootDirectory.FullName, ".git"));

				if (isWorkspaceRootDirectory)
				{
					return workspaceRootDirectory;
				}

				workspaceRootDirectory = workspaceRootDirectory.Parent;
			} while (workspaceRootDirectory != null && workspaceRootDirectory.Exists);

			throw new InvalidOperationException("Cannot find git repository root.");
		}
	}
}