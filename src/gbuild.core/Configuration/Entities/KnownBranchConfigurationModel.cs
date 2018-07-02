using GBuild.Configuration.Models;

namespace GBuild.Configuration.Entities
{
	public class KnownBranchConfigurationModel
	{
		public string Name { get; set; }
		public string Pattern { get; set; }
		public BranchType Type { get; set; }
		public BranchVersioningStrategyModel Versioning { get; set; } = new BranchVersioningStrategyModel();
	}
}