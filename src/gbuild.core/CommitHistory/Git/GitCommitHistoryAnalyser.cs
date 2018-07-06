using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using GBuild.Variables;
using GBuild.Vcs;
using Humanizer;
using LibGit2Sharp;
using Serilog;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	// TODO: remove dependency on Workspace, all workspace details should be passed to the analyser by the invoker
	// (current branch analysis context data provider, and soon the workspace context data provider)
	public class GitCommitHistoryAnalyser : ICommitHistoryAnalyser
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly IContextData<Workspace> _workspaceContextData;
		private readonly IBranchHistoryAnalyserProvider _branchHistoryAnalyserProvider;

		public GitCommitHistoryAnalyser(
			IWorkspaceConfiguration workspaceConfiguration,
			IContextData<Workspace> workspaceContextData,
			IBranchHistoryAnalyserProvider branchHistoryAnalyserProvider
			)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_workspaceContextData = workspaceContextData;
			_branchHistoryAnalyserProvider = branchHistoryAnalyserProvider;
		}

		public CommitHistoryAnalysis AnalyseCommitLog(
			string branchName			
		)
		{
			var workspace = _workspaceContextData.Data;
			var projectList = workspace.Projects.ToList();

			var knownBranch = _workspaceConfiguration.KnownBranches.First(k => k.IsMatch(branchName));
			var branchAnalysisSettings = knownBranch.AnalysisSettings;

			var branchHistoryAnalyser = _branchHistoryAnalyserProvider.GetBranchHistoryAnalyser(branchName);
			// libgit2sharp library takes care of comparing git trees together to evaluate changes.
			var commitsTowardsTarget = branchHistoryAnalyser.GetCommitsTowardsTarget(branchName, branchAnalysisSettings);
			var commitsAheadOfParent = branchHistoryAnalyser.GetCommitsAheadOfParent(branchName, branchAnalysisSettings);

			// determine changed modules
			var rootDirectory = new Uri(workspace.RepositoryRootDirectory.FullName.TrimEnd('\\') + "\\");

			var moduleRootDirectories = projectList.OfType<BaseCsharpProject>()
				.Select(m => new
					{
						Module = m,
						Uri = rootDirectory.MakeRelativeUri(new Uri(m.File.DirectoryName))
					}
				)
				.Select(m => new
				{
					m.Module,
					Path = Uri.UnescapeDataString(m.Uri.ToString())
				})
				.ToDictionary(m => m.Path, m => m.Module);

			var commitsTowardsTargetPerProject = projectList.ToDictionary( project=>project, project=>new List<Commit>());
			var commitsAheadOfParentPerProject = projectList.ToDictionary( project=>project, project=>new List<Commit>());
			var breakingChangesInProject = projectList.ToDictionary( project=>project, project=>false);
			var newFeaturesInProject = projectList.ToDictionary(project => project, project => false);

			foreach (var commit in commitsTowardsTarget)
			{
				foreach (var file in commit.ChangedFiles)
				{
					foreach (var rootDir in moduleRootDirectories)
					{
						if (!file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase))
						{
							continue;
						}
						
						var list = commitsTowardsTargetPerProject[rootDir.Value];
						if (!list.Contains(commit))
						{
							list.Add(commit);
						}
					}
				}
			}

			foreach (var commit in commitsAheadOfParent)
			{
				foreach (var file in commit.ChangedFiles)
				{
					foreach (var rootDir in moduleRootDirectories)
					{
						if (!file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase))
						{
							continue;
						}

						var list = commitsAheadOfParentPerProject[rootDir.Value];
						if (!list.Contains(commit))
						{
							list.Add(commit);
						}
					}
				}
			}

			var changedProjects = projectList.ToDictionary(
				project => project,
				project => new ChangedProject(
					commitsTowardsTargetPerProject[project],
					commitsAheadOfParentPerProject[project],
					breakingChangesInProject[project],
					newFeaturesInProject[project]
				));

			var files = commitsTowardsTarget.SelectMany(x => x.ChangedFiles).Distinct();

			return new CommitHistoryAnalysis(
				branchName,
				changedProjects,
				commitsTowardsTarget,
				files,
				false,
				false
			);
		}
	}
}