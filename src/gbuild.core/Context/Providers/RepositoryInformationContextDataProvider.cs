using System;
using System.IO;
using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Providers
{
	public abstract class RepositoryInformationContextDataProvider : IContextDataProvider<RepositoryInformation>
	{
		private readonly ConfigurationFile _configuration;
		private readonly IContextData<ProcessInformation> _processInformation;

		public RepositoryInformationContextDataProvider(
			ConfigurationFile configuration,
			IContextData<ProcessInformation> processInformation
		)
		{
			_configuration = configuration;
			_processInformation = processInformation;
		}

		protected IContextData<ProcessInformation> ProcessInformation => _processInformation;

		protected ConfigurationFile Configuration => _configuration;

		protected abstract DirectoryInfo GetRepositoryRootDirectory();

		public virtual RepositoryInformation LoadContextData()
		{
			var repositoryRootDirectory = GetRepositoryRootDirectory();

			var sourceCodeRootDirectory =
				new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, _configuration.SourceCodeRoot));

			if (!sourceCodeRootDirectory.Exists)
			{
				throw new InvalidOperationException("Source code directory not found");
			}

			var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);

			return new RepositoryInformation(
				repositoryRootDirectory,
				sourceCodeRootDirectory,
				projectFiles.Select(fi => new Module(fi.Name, fi, ModuleType.CSharp)).ToList()
			);
		}
	}
}