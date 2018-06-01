using System.Collections.Generic;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Models;
using LibGit2Sharp;

namespace GBuild.Core.Context.Data
{
	public class CommitAnalysisResult
	{
		public CommitAnalysisResult(
			IEnumerable<Project> changedModules,
			int numberOfChanges,
			bool hasBreakingChanges,
			bool hasNewFeatures
		)
		{
			ChangedModules = changedModules;
			NumberOfChanges = numberOfChanges;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
		}

		/// <summary>
		///     Modules changed in this branch.
		/// </summary>
		public IEnumerable<Project> ChangedModules { get; }

		public int NumberOfChanges { get; }

		public IBranchVersioningStrategyModel VersioningStrategyModel { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}