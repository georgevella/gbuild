using System.Collections.Generic;

namespace GBuild.Models
{
	public class ChangedProject
	{
		public ChangedProject(
			IEnumerable<Commit> commitsTowardsMerge,
			IEnumerable<Commit> commitsAheadOfParent,
			bool hasBreakingChanges,
			bool hasNewFeatures
		)
		{
			CommitsTowardsMerge = commitsTowardsMerge;
			CommitsAheadOfParent = commitsAheadOfParent;
			HasBreakingChanges = hasBreakingChanges;
			HasNewFeatures = hasNewFeatures;
		}

		/// <summary>
		///		All the commits that are associated with the project.
		/// </summary>
		public IEnumerable<Commit> CommitsTowardsMerge { get; }

		public IEnumerable<Commit> CommitsAheadOfParent { get; }

		public bool HasBreakingChanges { get; }

		public bool HasNewFeatures { get; }
	}
}