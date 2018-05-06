using System.Collections.Generic;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Data
{
	public class CommitAnalysis
	{
		public CommitAnalysis(
			IEnumerable<Module> changedModules,
			IEnumerable<Commit> commits,
			bool hasBreakingChanges,
			bool hasNewFeatures
		)
		{
			ChangedModules = changedModules;
			Commits = commits;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
		}

		/// <summary>
		///     Modules changed in this branch.
		/// </summary>
		public IEnumerable<Module> ChangedModules { get; }

		/// <summary>
		///     Commits submitted to a branch that are not present since the last tag or from a parent branch.
		/// </summary>
		public IEnumerable<Commit> Commits { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}