using System.Collections.Generic;
using System.Linq;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.CommitHistory
{
	class BranchHistoryAnalyserProvider : IBranchHistoryAnalyser
	{
		private readonly DevelopmentBranchHistoryAnalyser _branchHistoryAnalyser;

		public BranchHistoryAnalyserProvider(
			IContextData<Workspace> workspace,
			IEnumerable<IBranchHistoryAnalyser> branchHistoryAnalysers
		)
		{
			_branchHistoryAnalyser = branchHistoryAnalysers.OfType<DevelopmentBranchHistoryAnalyser>().FirstOrDefault();
		}
		public IEnumerable<Commit> GetCommitsTowardsTarget(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		)
		{
			return _branchHistoryAnalyser.GetCommitsTowardsTarget(branchName, branchAnalysisSettings);
		}

		public IEnumerable<Commit> GetCommitsAheadOfParent(
			string branchName,
			IBranchAnalysisSettings branchAnalysisSettings
		)
		{
			return _branchHistoryAnalyser.GetCommitsAheadOfParent(branchName, branchAnalysisSettings);
		}
	}
}