using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using GBuild.Vcs;
using LibGit2Sharp;
using Serilog;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	// TODO: remove dependency on Workspace, all workspace details should be passed to the analyser by the invoker
	// (current branch analysis context data provider, and soon the workspace context data provider)
	public class GitCommitHistoryAnalyser : ICommitHistoryAnalyser
	{
		private readonly IRepository _gitRepository;

		public GitCommitHistoryAnalyser(
			IRepository gitRepository
			)
		{
			_gitRepository = gitRepository;
		}

		public CommitHistoryAnalysis AnalyseCommitLog(IBranchHistoryAnalyser branchVersioningStrategy, DirectoryInfo repositoryRootDirectory, IEnumerable<Project> projects)
		{


			// libgit2sharp library takes care of comparing git trees together to evaluate changes.
			var commits = branchVersioningStrategy.GetNewCommits();

			// determine changed modules
			var rootDirectory = new Uri(repositoryRootDirectory.FullName.TrimEnd('\\') + "\\");

			var moduleRootDirectories = projects.OfType<BaseCsharpProject>()
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

			var commitsPerProject = projects.ToDictionary( project=>project, project=>new List<Commit>());
			var breakingChangesInProject = projects.ToDictionary( project=>project, project=>false);
			var newFeaturesInProject = projects.ToDictionary(project => project, project => false);

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

			var changedProjects = projects.ToDictionary(
				project => project,
				project => new ChangedProject(
					commitsPerProject[project],
					breakingChangesInProject[project],
					newFeaturesInProject[project]
				));

			var files = commits.SelectMany(x => x.ChangedFiles).Distinct();

			return new CommitHistoryAnalysis(
				changedProjects,
				commits,
				files,
				false,
				false
			);
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