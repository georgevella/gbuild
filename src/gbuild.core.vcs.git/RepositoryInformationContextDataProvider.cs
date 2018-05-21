using System;
using System.IO;
using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Context.Providers;
using GBuild.Core.Models;

namespace GBuild.Core.Vcs.Git
{
	public class GitRepositoryInformationContextDataProvider : ProjectInformationContextDataProvider
	{
		private readonly IConfigurationFile _configuration;
		private readonly IContextData<ProcessInformation> _processInformation;

		public GitRepositoryInformationContextDataProvider(
			IConfigurationFile configuration,
			IContextData<ProcessInformation> processInformation
		) : base(configuration, processInformation)
		{
			_configuration = configuration;
			_processInformation = processInformation;
		}

		protected override DirectoryInfo GetRepositoryRootDirectory()
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
	}
}