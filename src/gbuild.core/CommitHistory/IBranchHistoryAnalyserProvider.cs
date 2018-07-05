using GBuild.Configuration.Models;

namespace GBuild.CommitHistory
{
	public interface IBranchHistoryAnalyserProvider
	{
		IBranchHistoryAnalyser GetBranchHistoryAnalyser(
			string branchName
		);

		IBranchHistoryAnalyser GetBranchHistoryAnalyser(
			IKnownBranch knownBranch
		);
	}
}