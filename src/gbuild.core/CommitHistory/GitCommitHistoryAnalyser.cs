using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Context;
using GBuild.Models;
using GBuild.Vcs;
using LibGit2Sharp;

namespace GBuild.CommitHistory
{
	public class GitCommitHistoryAnalyser : ICommitHistoryAnalyser
	{
		private readonly IContextData<WorkspaceDescription> _workspace;
		private readonly IGitRepositoryHelpers _gitRepositoryHelpersHelpers;
		private readonly IRepository _gitRepository;

		public GitCommitHistoryAnalyser(
			IGitRepositoryHelpers gitRepositoryHelpersHelpers,
			IRepository gitRepository,
			IContextData<WorkspaceDescription> workspace)
		{
			_gitRepositoryHelpersHelpers = gitRepositoryHelpersHelpers;
			_gitRepository = gitRepository;
			_workspace = workspace;
		}

		public CommitHistoryAnalysis Run()
		{
			var currentBranch = _gitRepository.Branches.First(b => b.IsCurrentRepositoryHead);

			// TODO: how to handle branches that are not development / slaves of other branches
			var parentBranch = _gitRepository.Branches.First(b => b.CanonicalName == _workspace.Data.BranchVersioningStrategy.ParentBranch);

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

			var changedProjects = new Dictionary<Project, List<GBuild.Models.Commit>>();

			foreach (var commit in commits)
			{
				foreach (var file in commit.ChangedFiles)
				{
					foreach (var rootDir in moduleRootDirectories)
					{
						if (file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase))
						{
							if (!changedProjects.ContainsKey(rootDir.Value))
							{
								changedProjects.Add(
									rootDir.Value,
									new List<GBuild.Models.Commit>()
									{
										commit
									}
								);
							}
							else
							{
								changedProjects[rootDir.Value].Add(commit);
							}
						}
					}

				}
			}

			return new CommitHistoryAnalysis(
				changedProjects,
				commits,
				files.Select( f => new ChangedFile( f.Path )),
				false,
				false
			);
		}

		public IList<GBuild.Models.Commit> GetNewCommits(
			LibGit2Sharp.Branch sourceBranch,
			LibGit2Sharp.Branch branch
		)
		{
			var filter = new CommitFilter
			{
				ExcludeReachableFrom = sourceBranch,
				IncludeReachableFrom = branch,
				SortBy = CommitSortStrategies.Time
			};

			return _gitRepository.Commits.QueryBy(filter).Select(BuildCommitEntry).ToList();
		}

		private GBuild.Models.Commit BuildCommitEntry(LibGit2Sharp.Commit arg)
		{
			// TODO: store merge commit parental history
			var treeChanges = new List<TreeEntryChanges>();

			foreach (var parent in arg.Parents)
			{
				treeChanges.AddRange(_gitRepositoryHelpersHelpers.CompareTrees(parent.Tree, arg.Tree));
			}

			var changedFiles = treeChanges.Select(e => new ChangedFile(e.Path)).ToList();

			return new GBuild.Models.Commit(arg.Id.Sha, arg.Committer.Name, arg.Message, changedFiles);
		}

		public IEnumerable<TreeEntryChanges> GetChangedFiles(
			LibGit2Sharp.Branch parentBranch,
			LibGit2Sharp.Branch branch
		)
		{
			var filter = new CommitFilter
			{
				ExcludeReachableFrom = parentBranch,
				IncludeReachableFrom = branch,
				SortBy = CommitSortStrategies.Time
			};

			var commits = _gitRepository.Commits.QueryBy(filter).ToList();

			var newestCommit = commits.First();
			var oldestCommit = commits.Last();

			var treeChanges = _gitRepositoryHelpersHelpers.CompareTrees(oldestCommit.Tree, newestCommit.Tree);

			return treeChanges;
		}
	}
}