using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GBuild.Configuration.Models;

namespace GBuild.Models
{
	public class CommitHistoryAnalysis
	{
		public CommitHistoryAnalysis(
			string branchName,
			IDictionary<Project, ChangedProject> changedProjects,
			IEnumerable<Commit> commits,
			IEnumerable<ChangedFile> changedFiles,
			bool hasBreakingChanges,
			bool hasNewFeatures
			)
		{
			ChangedProjects = new ReadOnlyDictionary<Project, ChangedProject>(changedProjects);
			Commits = commits.ToList();
			ChangedFiles = changedFiles.ToList();
			BranchName = branchName;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
		}

		/// <summary>
		///     Projects changed in this branch.
		/// </summary>
		public IReadOnlyDictionary<Project, ChangedProject> ChangedProjects { get; }

		public IReadOnlyCollection<ChangedFile> ChangedFiles { get; }

		public IReadOnlyCollection<Commit> Commits { get; }

		public string BranchName { get; }
		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}