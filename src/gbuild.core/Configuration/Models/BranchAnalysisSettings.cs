namespace GBuild.Configuration.Models
{
	class BranchAnalysisSettings : IBranchAnalysisSettings
	{
		public string ParentBranch { get; set; }
		public string MergeTarget { get; set; }
	}
}