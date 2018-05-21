using System;
using System.IO;
using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Providers
{
	public abstract class ProjectInformationContextDataProvider : IContextDataProvider<ProjectInformation>
	{
		private readonly IConfigurationFile _configuration;
		private readonly IContextData<ProcessInformation> _processInformation;

		protected ProjectInformationContextDataProvider(
			IConfigurationFile configuration,
			IContextData<ProcessInformation> processInformation
		)
		{
			_configuration = configuration;
			_processInformation = processInformation;
		}

		protected IContextData<ProcessInformation> ProcessInformation => _processInformation;

		protected IConfigurationFile Configuration => _configuration;

		protected abstract DirectoryInfo GetRepositoryRootDirectory();

		public virtual ProjectInformation LoadContextData()
		{
			var repositoryRootDirectory = GetRepositoryRootDirectory();

			var sourceCodeRootDirectory =
				new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, _configuration.SourceCodeRoot));

			if (!sourceCodeRootDirectory.Exists)
			{
				throw new InvalidOperationException("Source code directory not found");
			}

			var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);

			return new ProjectInformation(
				repositoryRootDirectory,
				sourceCodeRootDirectory,
				projectFiles.Select(fi => new CsharpModule(fi.Name, fi, ModuleType.CSharp)).ToList()
			);
		}
	}
}