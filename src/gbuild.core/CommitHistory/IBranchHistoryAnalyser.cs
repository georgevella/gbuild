using System.Collections.Generic;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.CommitHistory
{
	public interface IBranchHistoryAnalyser
	{
		IEnumerable<Commit> GetCommitsTowardsTarget(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		);

		IEnumerable<Commit> GetCommitsAheadOfParent(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		);
	}
}