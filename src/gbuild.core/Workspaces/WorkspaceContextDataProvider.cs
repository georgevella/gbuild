using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GBuild.Configuration;
using GBuild.Context;
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

		public WorkspaceContextDataProvider(
			IWorkspaceConfiguration configuration,
			IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider,
			IWorkspaceSourceCodeDirectoryProvider workspaceSourceCodeDirectoryProvider,
			IProjectDiscoveryService projectDiscoveryService,			 
			IRepository repository
		)
		{
			_configuration = configuration;
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
			_workspaceSourceCodeDirectoryProvider = workspaceSourceCodeDirectoryProvider;
			_projectDiscoveryService = projectDiscoveryService;
			_repository = repository;
		}

		public Workspace LoadContextData()
		{
			var workspaceRootDirectory = _workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();

			var sourceCodeRootDirectory = _workspaceSourceCodeDirectoryProvider.GetSourceCodeDirectory();

			// determine all project files
			//var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
			var projects = _projectDiscoveryService.GetProjects();

			// determine branch version strategy
			var currentBranch = _repository.Branches.First(b => b.IsCurrentRepositoryHead);

			var branch = _configuration.KnownBranches.FirstOrDefault(b => b.IsMatch(currentBranch.CanonicalName));
			if (branch == null)
				throw new Exception("Could not determine branch version strategy from current branch");

			var branchVersioningStrategy = branch.VersioningStrategy;			

			return new Workspace(
				workspaceRootDirectory,
				sourceCodeRootDirectory,
				projects,
				branchVersioningStrategy,
				BuildWorkspaceVariables(currentBranch)
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