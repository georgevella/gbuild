namespace GBuild.Configuration.Models
{
	public interface IKnownBranch
	{
		string Name { get; }

		IBranchVersioningStrategy VersioningStrategy { get; }

		BranchType Type { get; }

		bool IsMatch(
			string branchName
		);
	}
}