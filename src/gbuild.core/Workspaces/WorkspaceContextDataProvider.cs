using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GBuild.CommitHistory;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using GBuild.Projects.Discovery;
using GBuild.ReleaseHistory;
using GBuild.Variables;
using Humanizer;
using LibGit2Sharp;

namespace GBuild.Workspaces
{
	public class WorkspaceContextDataProvider : IContextDataProvider<Workspace>
	{
		private readonly IWorkspaceConfiguration _configuration;
		private readonly IWorkspaceRootDirectoryProvider _workspaceRootDirectoryProvider;
		private readonly IWorkspaceSourceCodeDirectoryProvider _workspaceSourceCodeDirectoryProvider;
		private readonly IProjectDiscoveryService _projectDiscoveryService;		
		private readonly IRepository _repository;
		private readonly IServiceProvider _serviceProvider;
		private readonly IVariableStore _variableStore;

		public WorkspaceContextDataProvider(
			IWorkspaceConfiguration configuration,
			IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider,
			IWorkspaceSourceCodeDirectoryProvider workspaceSourceCodeDirectoryProvider,
			IProjectDiscoveryService projectDiscoveryService,			 
			IRepository repository,
			IServiceProvider serviceProvider,
			IVariableStore variableStore
		)
		{
			_configuration = configuration;
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
			_workspaceSourceCodeDirectoryProvider = workspaceSourceCodeDirectoryProvider;
			_projectDiscoveryService = projectDiscoveryService;
			_repository = repository;
			_serviceProvider = serviceProvider;
			_variableStore = variableStore;
		}

		public Workspace LoadContextData()
		{
			var workspaceRootDirectory = _workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();

			var sourceCodeRootDirectory = _workspaceSourceCodeDirectoryProvider.GetSourceCodeDirectory();

			// determine all project files
			//var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
			var projects = _projectDiscoveryService.GetProjects().ToList();

			// determine branch version strategy
			var currentBranch = _repository.GetCurrentBranch();

			var branch = _configuration.KnownBranches.FirstOrDefault(b => b.IsMatch(currentBranch.CanonicalName));
			if (branch == null)
				throw new Exception("Could not determine branch version strategy from current branch");

//			IBranchHistoryAnalyser branchHistoryAnalyser = null;
//			switch (branch.Type)
//			{
//				case BranchType.Development:
//					branchHistoryAnalyser = (IBranchHistoryAnalyser)_serviceProvider.GetService(typeof(DevelopmentBranchHistoryAnalyser));
//					break;
//				case BranchType.Release:
//					branchHistoryAnalyser = (IBranchHistoryAnalyser)_serviceProvider.GetService(typeof(ReleaseBranchHistoryAnalyser));
//					break;
//			}

			projects.ForEach( p => _variableStore.AddProject(p));

			foreach (var pair in BuildWorkspaceVariables(currentBranch))
			{
				_variableStore.Global[pair.Key] = pair.Value;
			}

			return new Workspace(
				workspaceRootDirectory,
				sourceCodeRootDirectory,
				projects,
				branch
			);
		}

		private IDictionary<string, string> BuildWorkspaceVariables(Branch currentBranch)
		{
			return new Dictionary<string, string>()
			{
				{WorkspaceVariables.BranchName, currentBranch.FriendlyName},
				{WorkspaceVariables.FeatureName, GetFeatureNameFromBranch(currentBranch)},
				{WorkspaceVariables.IssueId, GetIssueIdFromBranch(currentBranch)}
			};
		}

		private string GetIssueIdFromBranch(
			Branch currentBranch
		)
		{
			// TODO: add support
			return String.Empty;
		}

		private string GetFeatureNameFromBranch(
			Branch currentBranch
		)
		{
			if (currentBranch.CanonicalName.StartsWith("refs/heads/feature/"))
			{
				var featureName = currentBranch.CanonicalName.Substring("refs/heads/feature/".Length);
				featureName = new String(featureName.Where(c => c != '-' && c != '_').ToArray()).Transform(To.LowerCase)
					.Truncate(10, string.Empty);
				return featureName;
			}

			return string.Empty;
		}

	}
}