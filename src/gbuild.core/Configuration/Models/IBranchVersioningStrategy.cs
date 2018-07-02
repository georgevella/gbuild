using GBuild.Configuration.Entities;

namespace GBuild.Configuration.Models
{
	public interface IBranchVersioningStrategy
	{
		string Name { get; }

		/// <summary>
		///     Another branch in the repository that will be tracked for version information.
		/// </summary>
		string ParentBranch { get; }

		/// <summary>
		///     Pre-release tag.
		/// </summary>
		string Tag { get; }

		string Metadata { get; }

		VersionIncrementStrategy Increment { get; }
	}
}