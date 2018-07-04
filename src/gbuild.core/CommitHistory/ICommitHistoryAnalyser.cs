using System.Collections.Generic;
using System.IO;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.CommitHistory
{
	public interface ICommitHistoryAnalyser
	{
		CommitHistoryAnalysis AnalyseCommitLog(
			IBranchHistoryAnalyser branchHistoryAnalyser,
			IBranchAnalysisSettings branchAnalysisSettings,
			string branchName
		);
	}
}