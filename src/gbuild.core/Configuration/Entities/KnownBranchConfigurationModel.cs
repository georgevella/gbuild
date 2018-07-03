using GBuild.Configuration.Models;

namespace GBuild.Configuration.Entities
{
	public class KnownBranchConfigurationModel
	{
		public string Name { get; set; }
		public string Pattern { get; set; }
		public BranchType Type { get; set; }
		public BranchVersioningModel Versioning { get; set; } = new BranchVersioningModel();

		public BranchAnalysisModel Analysis { get; set; } = new BranchAnalysisModel();
	}
}