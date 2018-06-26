using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.ReleaseHistory;
using GBuild.Workspace;
using LibGit2Sharp;

namespace GBuild.Context.Providers
{
	public class WorkspaceContextDataProvider : IContextDataProvider<WorkspaceDescription>
	{
		private readonly IWorkspaceConfiguration _configuration;
		private readonly IWorkspaceRootDirectoryProvider _workspaceRootDirectoryProvider;
		private readonly IContextData<Process> _processInformation;
		private readonly IReleaseHistoryProvider _releaseHistoryProvider;
		private readonly IRepository _sourceCodeRepository;

		public WorkspaceContextDataProvider(
			IWorkspaceConfiguration configuration,
			IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider,
			IContextData<Process> processInformation,
			IReleaseHistoryProvider releaseHistoryProvider, 
			IRepository sourceCodeRepository
		)
		{
			_configuration = configuration;
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
			_processInformation = processInformation;
			_releaseHistoryProvider = releaseHistoryProvider;
			_sourceCodeRepository = sourceCodeRepository;
		}

		public WorkspaceDescription LoadContextData()
		{
			var workspaceRootDirectory = _workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();

			var sourceCodeRootDirectory =
				new DirectoryInfo(Path.Combine(workspaceRootDirectory.FullName, _configuration.SourceCodeRoot));

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

			return new WorkspaceDescription(
				workspaceRootDirectory,
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