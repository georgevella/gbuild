namespace GBuild.Core.Configuration.Models
{
	public interface IBranchVersioningStrategyModel
	{
		string Name { get; set; }

		/// <summary>
		///     Another branch in the repository that will be tracked for version information.
		/// </summary>
		string ParentBranch { get; set; }

		/// <summary>
		///     Pre-release tag.
		/// </summary>
		string Tag { get; set; }

		string Metadata { get; set; }

		VersionIncrementStrategy Increment { get; set; }
	}
}