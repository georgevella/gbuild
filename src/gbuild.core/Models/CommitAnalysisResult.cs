using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Models;
using LibGit2Sharp;
using Commit = GBuild.Core.Models.Commit;

namespace GBuild.Core.Context.Data
{
	public class CommitAnalysisResult
	{
		public CommitAnalysisResult(
			IDictionary<Project, List<Commit>> changedProjects,
			IEnumerable<Commit> commits,
			IEnumerable<ChangedFile> changedFiles,
			bool hasBreakingChanges,
			bool hasNewFeatures,
			IBranchVersioningStrategyModel versioningStrategyModel)
		{
			ChangedProjects = new ReadOnlyDictionary<Project, List<Commit>>(changedProjects);
			Commits = commits.ToList();
			ChangedFiles = changedFiles;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
			VersioningStrategyModel = versioningStrategyModel;
		}

		/// <summary>
		///     Projects changed in this branch.
		/// </summary>
		public IReadOnlyDictionary<Project, List<Commit>> ChangedProjects { get; }

		public IEnumerable<ChangedFile> ChangedFiles { get; }

		public IReadOnlyCollection<Commit> Commits { get; }

		public IBranchVersioningStrategyModel VersioningStrategyModel { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}