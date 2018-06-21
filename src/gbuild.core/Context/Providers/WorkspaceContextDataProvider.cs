using System;
using System.IO;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.Context.Providers
{
	public class WorkspaceContextDataProvider : IContextDataProvider<Workspace>
	{
		private readonly IConfigurationFile _configuration;
		private readonly IContextData<Process> _processInformation;
		private readonly IContextData<Models.ReleaseHistory> _releaseHistory;

		public WorkspaceContextDataProvider(
			IConfigurationFile configuration,
			IContextData<Process> processInformation,
			IContextData<Models.ReleaseHistory> releaseHistory
		)
		{
			_configuration = configuration;
			_processInformation = processInformation;
			_releaseHistory = releaseHistory;
		}

		private DirectoryInfo GetRepositoryRootDirectory()
		{
			var repositoryRootDirectory = _processInformation.Data.CurrentDirectory;
			var dotGitDirectory = new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
			while (!dotGitDirectory.Exists && repositoryRootDirectory.Parent != null)
			{
				repositoryRootDirectory = repositoryRootDirectory.Parent;
				dotGitDirectory = new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
			}

			return repositoryRootDirectory;
		}


		public Workspace LoadContextData()
		{
			var repositoryRootDirectory = GetRepositoryRootDirectory();

			var sourceCodeRootDirectory =
				new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, _configuration.SourceCodeRoot));

			if (!sourceCodeRootDirectory.Exists)
			{
				throw new InvalidOperationException("Source code directory not found");
			}

			var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);

			return new Workspace(
				repositoryRootDirectory,
				sourceCodeRootDirectory,
				projectFiles.Select(fi => new CsharpProject(fi.Name, fi, ModuleType.CSharp)).ToList(),
				_releaseHistory.Data.Releases
			);
		}
	}
}