using GBuild.CommitHistory;
using GBuild.Models;

namespace GBuild.Context.Providers
{
	public class CommitAnalysisContextDataProvider : IContextDataProvider<CommitHistoryAnalysis>
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;
		private readonly IContextData<Workspace> _workspaceContextData;


		public CommitAnalysisContextDataProvider(
			ICommitHistoryAnalyser commitHistoryAnalyser,
			IContextData<Workspace> workspaceContextData
		)
		{
			_commitHistoryAnalyser = commitHistoryAnalyser;
			_workspaceContextData = workspaceContextData;
		}

		public CommitHistoryAnalysis LoadContextData()
		{
			return _commitHistoryAnalyser.AnalyseCommitLog(
				_workspaceContextData.Data.BranchVersioningStrategy,										   
				_workspaceContextData.Data.RepositoryRootDirectory, 
				_workspaceContextData.Data.Projects);
		}
	}
}