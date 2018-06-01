using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Exceptions;
using GBuild.Core.Models;
using LibGit2Sharp;

namespace GBuild.Core.CommitAnalysis.Git
{
	public class GitCommitHistoryAnalyser : ICommitHistoryAnalyser
	{
		private readonly IContextData<Workspace> _workspace;
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly IGitRepository _sourceCodeRepository;

		public GitCommitHistoryAnalyser(
			IWorkspaceConfiguration workspaceConfiguration,
			IGitRepository sourceCodeRepository,
			IContextData<Workspace> workspace)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_sourceCodeRepository = sourceCodeRepository;
			_workspace = workspace;
		}

		public CommitAnalysisResult Run()
		{
			var currentBranch = _sourceCodeRepository.Branches.First(b => b.IsCurrentRepositoryHead);

			var branchVersioningStrategy =
				_workspaceConfiguration.BranchVersioningStrategies.FirstOrDefault(b => MatchesCurrentBranch(currentBranch, b.Name));

			if (branchVersioningStrategy == null)
				throw new CommitAnalysisException("Could not determine branch version strategy from current branch");

			// TODO: how to handle branches that are not development / slaves of other branches
			var parentBranch = _sourceCodeRepository.Branches.First(b => b.CanonicalName == branchVersioningStrategy.ParentBranch);

			var commits = GetNewCommits(
				parentBranch,
				currentBranch
			);

			// determine changed files
			var files = GetChangedFiles(
				parentBranch,
				currentBranch
			);

			// determine changed modules
			var rootDirectory = new Uri(_workspace.Data.RepositoryRootDirectory.FullName.TrimEnd('\\') + "\\");

			var moduleRootDirectories = _workspace.Data.Modules.OfType<CsharpProject>()
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

			var changedModules = new List<Project>();

			foreach (var file in files)
			foreach (var rootDir in moduleRootDirectories)
			{
				if (file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase) &&
					!changedModules.Contains(rootDir.Value))
				{
					changedModules.Add(rootDir.Value);
				}
			}

			return new CommitAnalysisResult(
				changedModules,
				commits.Count(),
				false,
				false
			);
		}

		public IEnumerable<Commit> GetNewCommits(
			Branch sourceBranch,
			Branch branch
		)
		{
			var filter = new CommitFilter
			{
				ExcludeReachableFrom = sourceBranch,
				IncludeReachableFrom = branch,
				SortBy = CommitSortStrategies.Time
			};

			return _sourceCodeRepository.Commits.QueryBy(filter).Cast<Commit>();
		}

		public IEnumerable<TreeEntryChanges> GetChangedFiles(
			Branch parentBranch,
			Branch branch
		)
		{
			var filter = new CommitFilter
			{
				ExcludeReachableFrom = parentBranch,
				IncludeReachableFrom = branch,
				SortBy = CommitSortStrategies.Time
			};

			var commits = _sourceCodeRepository.Commits.QueryBy(filter).ToList();

			var newestCommit = commits.First();
			var oldestCommit = commits.Last();

			var treeChanges = _sourceCodeRepository.CompareTrees(oldestCommit.Tree, newestCommit.Tree);

			return treeChanges;
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

			// TODO: pattern matching branch name
			return false;
		}
	}
}