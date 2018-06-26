using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.ReleaseHistory;
using LibGit2Sharp;

namespace GBuild.Context.Providers
{
	public class WorkspaceContextDataProvider : IContextDataProvider<Workspace>
	{
		private readonly IWorkspaceConfiguration _configuration;
		private readonly IContextData<Process> _processInformation;
		private readonly IReleaseHistoryProvider _releaseHistoryProvider;
		private readonly IRepository _sourceCodeRepository;

		public WorkspaceContextDataProvider(
			IWorkspaceConfiguration configuration,

			IContextData<Process> processInformation,
			IReleaseHistoryProvider releaseHistoryProvider, 
			IRepository sourceCodeRepository
		)
		{
			_configuration = configuration;
			_processInformation = processInformation;
			_releaseHistoryProvider = releaseHistoryProvider;
			_sourceCodeRepository = sourceCodeRepository;
		}

		private DirectoryInfo GetRepositoryRootDirectory()
		{
			var repositoryRootDirectory = _processInformation.Data.CurrentDirectory;
			var dotGitDirectory = new FileInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
			while (!dotGitDirectory.Exists && repositoryRootDirectory.Parent != null)
			{
				repositoryRootDirectory = repositoryRootDirectory.Parent;
				dotGitDirectory = new FileInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
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

			// determine all project files
			var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);

			// determine branch version strategy
			var currentBranch = _sourceCodeRepository.Branches.First(b => b.IsCurrentRepositoryHead);

			var branchVersioningStrategy =
				_configuration.BranchVersioningStrategies.FirstOrDefault(b => MatchesCurrentBranch(currentBranch, b.Name));

			if (branchVersioningStrategy == null)
				throw new Exception("Could not determine branch version strategy from current branch");

			return new Workspace(
				repositoryRootDirectory,
				sourceCodeRootDirectory,
				projectFiles.Select(fi => new CsharpProject(Path.GetFileNameWithoutExtension(fi.Name), fi, ModuleType.CSharp)).ToList(),
				_releaseHistoryProvider.GetAllReleases(),
				branchVersioningStrategy
			);
		}

		private bool MatchesCurrentBranch(
			Branch currentBranch,
			string filter
		)
		{
			if (currentBranch.CanonicalName == filter)
			{
				return true;
			}

			try
			{
				if (Regex.IsMatch(currentBranch.CanonicalName, filter))
				{
					return true;
				}
			}
			catch {  }
			

			// TODO: pattern matching branch name
			return false;
		}
	}
}