﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Projects.Discovery;
using GBuild.ReleaseHistory;
using GBuild.Workspace;
using LibGit2Sharp;

namespace GBuild.Context.Providers
{
	public class WorkspaceContextDataProvider : IContextDataProvider<WorkspaceDescription>
	{
		private readonly IWorkspaceConfiguration _configuration;
		private readonly IWorkspaceRootDirectoryProvider _workspaceRootDirectoryProvider;
		private readonly IWorkspaceSourceCodeDirectoryProvider _workspaceSourceCodeDirectoryProvider;
		private readonly IProjectDiscoveryService _projectDiscoveryService;
		private readonly IReleaseHistoryProvider _releaseHistoryProvider;
		private readonly IRepository _sourceCodeRepository;

		public WorkspaceContextDataProvider(
			IWorkspaceConfiguration configuration,
			IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider,
			IWorkspaceSourceCodeDirectoryProvider workspaceSourceCodeDirectoryProvider,
			IProjectDiscoveryService projectDiscoveryService,
			IReleaseHistoryProvider releaseHistoryProvider, 
			IRepository sourceCodeRepository
		)
		{
			_configuration = configuration;
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
			_workspaceSourceCodeDirectoryProvider = workspaceSourceCodeDirectoryProvider;
			_projectDiscoveryService = projectDiscoveryService;
			_releaseHistoryProvider = releaseHistoryProvider;
			_sourceCodeRepository = sourceCodeRepository;
		}

		public WorkspaceDescription LoadContextData()
		{
			var workspaceRootDirectory = _workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();

			var sourceCodeRootDirectory = _workspaceSourceCodeDirectoryProvider.GetSourceCodeDirectory();

			// determine all project files
			//var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
			var projects = _projectDiscoveryService.GetProjects();

			// determine branch version strategy
			var currentBranch = _sourceCodeRepository.Branches.First(b => b.IsCurrentRepositoryHead);

			var branchVersioningStrategy =
				_configuration.BranchVersioningStrategies.FirstOrDefault(b => MatchesCurrentBranch(currentBranch, b.Name));

			if (branchVersioningStrategy == null)
				throw new Exception("Could not determine branch version strategy from current branch");

			var releases = _releaseHistoryProvider.GetAllReleases();
			var latestRelease = _releaseHistoryProvider.GetLatestRelease();
			var currentVersionNumbers = latestRelease?.VersionNumbers ?? WorkspaceVersionInfo.Empty();

			return new WorkspaceDescription(
				workspaceRootDirectory,
				sourceCodeRootDirectory,
				projects,
				releases,				
				branchVersioningStrategy,
				currentVersionNumbers
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