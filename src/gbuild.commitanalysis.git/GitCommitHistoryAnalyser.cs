using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.CommitHistoryAnalyser;
using GBuild.Configuration;
using GBuild.Context;
using GBuild.Exceptions;
using GBuild.Models;
using LibGit2Sharp;
using Branch = gbuild.commitanalysis.git.Models.Branch;
using Commit = gbuild.commitanalysis.git.Models.Commit;

namespace gbuild.commitanalysis.git
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

		public CommitHistoryAnalysis Run()
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

			var moduleRootDirectories = _workspace.Data.Projects.OfType<CsharpProject>()
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

			var changedModules = new Dictionary<Project, List<GBuild.Models.Commit>>();

			foreach (var commit in commits)
			{
				foreach (var file in commit.ChangedFiles)
				{
					foreach (var rootDir in moduleRootDirectories)
					{
						if (file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase))
						{
							if (!changedModules.ContainsKey(rootDir.Value))
							{
								changedModules.Add(
									rootDir.Value,
									new List<GBuild.Models.Commit>()
									{
										commit
									}
								);
							}
							else
							{
								changedModules[rootDir.Value].Add(commit);
							}
						}
					}

				}
			}

			return new CommitHistoryAnalysis(
				changedModules,
				commits,
				files.Select( f => new ChangedFile( f.Path )),
				false,
				false,
				branchVersioningStrategy
			);
		}

		public IList<GBuild.Models.Commit> GetNewCommits(
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

			return _sourceCodeRepository.Commits.QueryBy(filter).Select(BuildCommitEntry).ToList();
		}

		private GBuild.Models.Commit BuildCommitEntry(LibGit2Sharp.Commit arg)
		{
			var treeChanges = _sourceCodeRepository.CompareTrees(arg.Parents.Single().Tree, arg.Tree);

			Commit commit = arg;
			var changedFiles = treeChanges.Select(e => new ChangedFile(e.Path)).ToList();

			return new GBuild.Models.Commit(commit.Id, commit.Committer.Name, commit.Message, changedFiles);
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