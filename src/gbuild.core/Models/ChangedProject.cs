using System.Collections.Generic;

namespace GBuild.Models
{
	public class ChangedProject
	{
		public ChangedProject(
			IEnumerable<Commit> commits,
			bool hasBreakingChanges,
			bool hasNewFeatures
		)
		{
			Commits = commits;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
		}

		public IEnumerable<Commit> Commits { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}