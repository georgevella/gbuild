namespace GBuild.Core.Configuration
{
	public class BranchVersioningStrategy : IBranchVersioningStrategy
	{
		public BranchVersioningStrategy()
		{
			Tag = string.Empty;
			Metadata = string.Empty;
		}
		
		public string Name { get; set; }
		
		public string ParentBranch { get; set; }

		public string Tag { get; set; }

		public string Metadata { get; set; }

		public VersionIncrementStrategy Increment { get; set; }
	}

	public interface IBranchVersioningStrategy
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