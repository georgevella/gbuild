namespace GBuild.Configuration.Models
{
	public interface IKnownBranch
	{
		string Name { get; }

		BranchType Type { get; }
		IBranchVersioningSettings VersioningSettings { get; }		
		IBranchAnalysisSettings AnalysisSettings { get; }
		bool IsMatch(
			string branchName
		);
	}
}