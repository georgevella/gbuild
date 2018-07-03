using GBuild.Configuration.Entities;

namespace GBuild.Configuration.Models
{
	public class BranchVersioningSettings : IBranchVersioningSettings
	{
		public string Tag { get; set; }
		public string Metadata { get; set; }
		public VersionIncrementStrategy Increment { get; set; }
	}
}