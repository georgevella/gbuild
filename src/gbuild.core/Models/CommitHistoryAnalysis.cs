using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GBuild.Configuration.Models;

namespace GBuild.Models
{
	public class CommitHistoryAnalysis
	{
		public CommitHistoryAnalysis(
			IDictionary<Project, List<Commit>> changedProjects,
			IEnumerable<Commit> commits,
			IEnumerable<ChangedFile> changedFiles,
			bool hasBreakingChanges,
			bool hasNewFeatures,
			IBranchVersioningStrategyModel versioningStrategyModel,
			IEnumerable<Release> releases
			)
		{
			ChangedProjects = new ReadOnlyDictionary<Project, List<Commit>>(changedProjects);
			Commits = commits.ToList();
			ChangedFiles = changedFiles.ToList();
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
			VersioningStrategyModel = versioningStrategyModel;
			Releases = releases.ToList();
		}

		/// <summary>
		///     Projects changed in this branch.
		/// </summary>
		public IReadOnlyDictionary<Project, List<Commit>> ChangedProjects { get; }

		public IReadOnlyCollection<ChangedFile> ChangedFiles { get; }

		public IReadOnlyCollection<Commit> Commits { get; }

		public IBranchVersioningStrategyModel VersioningStrategyModel { get; }

		public IReadOnlyCollection<Release> Releases { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}