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

		public WorkspaceContextDataProvider(
			IWorkspaceConfiguration configuration,
			IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider,
			IWorkspaceSourceCodeDirectoryProvider workspaceSourceCodeDirectoryProvider,
			IProjectDiscoveryService projectDiscoveryService,			 
			IRepository repository,
			IServiceProvider serviceProvider
		)
		{
			_configuration = configuration;
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
			_workspaceSourceCodeDirectoryProvider = workspaceSourceCodeDirectoryProvider;
			_projectDiscoveryService = projectDiscoveryService;
			_repository = repository;
			_serviceProvider = serviceProvider;
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

			return new Workspace(
				workspaceRootDirectory,
				sourceCodeRootDirectory,
				projects,
				branch
			);
		}

	}
}