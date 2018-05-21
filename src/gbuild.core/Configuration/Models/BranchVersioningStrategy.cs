namespace GBuild.Core.Configuration.Models
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
}