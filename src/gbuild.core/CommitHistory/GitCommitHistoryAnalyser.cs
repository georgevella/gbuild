using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Context;
using GBuild.Models;
using GBuild.Vcs;
using LibGit2Sharp;
using Serilog;
using Commit = GBuild.Models.Commit;

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

			Log.Debug("Commit analysis running between current branch [{currentbranch}] and [{parentbranch}:{parentcommit}]",
				currentBranch.Tip.Sha,
				_workspace.Data.BranchVersioningStrategy.ParentBranch,
				parentBranch.Tip.Sha);

			// libgit2sharp library takes care of comparing git trees together to evaluate changes.
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

			var moduleRootDirectories = _workspace.Data.Projects.OfType<BaseCsharpProject>()
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

			var commitsPerProject = _workspace.Data.Projects.ToDictionary( project=>project, project=>new List<Commit>());
			var breakingChangesInProject = _workspace.Data.Projects.ToDictionary( project=>project, project=>false);
			var newFeaturesInProject = _workspace.Data.Projects.ToDictionary(project => project, project => false);

			foreach (var commit in commits)
			{
				foreach (var file in commit.ChangedFiles)
				{
					foreach (var rootDir in moduleRootDirectories)
					{
						if (!file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase))
						{
							continue;
						}
						
						var list = commitsPerProject[rootDir.Value];
						if (!list.Contains(commit))
						{
							list.Add(commit);
						}
					}
				}
			}

			var changedProjects = _workspace.Data.Projects.ToDictionary(
				project => project,
				project => new ChangedProject(
					commitsPerProject[project],
					breakingChangesInProject[project],
					newFeaturesInProject[project]
				));

			return new CommitHistoryAnalysis(
				changedProjects,
				commits,
				files.Select( f => new ChangedFile( f.Path )),
				false,
				false
			);
		}

		public IList<Commit> GetNewCommits(
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

			return _gitRepository.Commits.QueryBy(filter).Select(BuildCommitEntry).ToList();
		}

		private Commit BuildCommitEntry(LibGit2Sharp.Commit arg)
		{
			// TODO: store merge commit parental history
			var treeChanges = new List<TreeEntryChanges>();

			foreach (var parent in arg.Parents)
			{
				treeChanges.AddRange(
					_gitRepository.Diff.Compare<TreeChanges>(parent.Tree, arg.Tree)
				);
			}

			var changedFiles = treeChanges.Select(e => new ChangedFile(e.Path)).ToList();

			return new Commit(arg.Id.Sha, arg.Committer.Name, arg.Message, changedFiles);
		}

		public IEnumerable<TreeEntryChanges> GetChangedFiles(
			Branch parentBranch,
			Branch branch
		)
		{
			var treeChanges = _gitRepository.Diff.Compare<TreeChanges>(parentBranch.Tip.Tree, branch.Tip.Tree);

			return treeChanges;
		}
	}
}