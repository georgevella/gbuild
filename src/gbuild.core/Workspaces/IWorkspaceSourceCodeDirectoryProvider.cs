using System;
using System.IO;
using GBuild.Configuration;

namespace GBuild.Workspaces
{
	public interface IWorkspaceSourceCodeDirectoryProvider
	{
		DirectoryInfo GetSourceCodeDirectory();
	}

	class WorkspaceSourceCodeDirectoryProvider : IWorkspaceSourceCodeDirectoryProvider
	{
		private readonly IWorkspaceConfiguration _configuration;
		private readonly IWorkspaceRootDirectoryProvider _workspaceRootDirectoryProvider;

		public WorkspaceSourceCodeDirectoryProvider(
			IWorkspaceConfiguration configuration,
			IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider
			)
		{
			_configuration = configuration;
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
		}
		/// <inheritdoc />
		public DirectoryInfo GetSourceCodeDirectory()
		{
			var workspaceRootDirectory = _workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();

			var sourceCodeRootDirectory = new DirectoryInfo(Path.Combine(workspaceRootDirectory.FullName, _configuration.SourceCodeRoot));

			if (!sourceCodeRootDirectory.Exists)
			{
				throw new InvalidOperationException("Source code directory not found");
			}

			return sourceCodeRootDirectory;
		}
	}
}