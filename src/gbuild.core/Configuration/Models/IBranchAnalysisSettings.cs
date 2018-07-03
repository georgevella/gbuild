namespace GBuild.Configuration.Models
{
	public interface IBranchAnalysisSettings
	{
		/// <summary>
		///		Indicates the parent branch / off shoot of.
		/// </summary>
		string ParentBranch { get; }

		/// <summary>
		///		Into which branch the current branch will be merged to.
		/// </summary>
		string MergeTarget { get; }
	}
}