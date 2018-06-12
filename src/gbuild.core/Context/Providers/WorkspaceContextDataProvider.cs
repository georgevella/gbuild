using System;
using System.IO;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.Context.Providers
{
	public abstract class WorkspaceContextDataProvider : IContextDataProvider<Workspace>
	{
		private readonly IConfigurationFile _configuration;
		private readonly IContextData<Process> _processInformation;

		protected WorkspaceContextDataProvider(
			IConfigurationFile configuration,
			IContextData<Process> processInformation
		)
		{
			_configuration = configuration;
			_processInformation = processInformation;
		}

		protected IContextData<Process> ProcessInformation => _processInformation;

		protected IConfigurationFile Configuration => _configuration;

		protected abstract DirectoryInfo GetRepositoryRootDirectory();

		public virtual Workspace LoadContextData()
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
				projectFiles.Select(fi => new CsharpProject(fi.Name, fi, ModuleType.CSharp)).ToList()
			);
		}
	}
}