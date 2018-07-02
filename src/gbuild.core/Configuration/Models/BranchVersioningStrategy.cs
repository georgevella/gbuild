using GBuild.Configuration.Entities;

namespace GBuild.Configuration.Models
{
	public class BranchVersioningStrategy : IBranchVersioningStrategy
	{
		public string Name { get; set; }
		public string ParentBranch { get; set; }
		public string Tag { get; set; }
		public string Metadata { get; set; }
		public VersionIncrementStrategy Increment { get; set; }
	}
}