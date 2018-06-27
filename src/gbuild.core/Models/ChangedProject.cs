using System.Collections.Generic;

namespace GBuild.Models
{
	public class ChangedProject
	{
		public ChangedProject(
			List<Commit> commits,
			bool hasBreakingChanges,
			bool hasNewFeatures
		)
		{
			Commits = commits;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
		}

		public List<Commit> Commits { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}