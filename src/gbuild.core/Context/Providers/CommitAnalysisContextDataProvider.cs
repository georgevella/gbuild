using GBuild.CommitHistoryAnalyser;
using GBuild.Models;

namespace GBuild.Context.Providers
{
	public class CommitAnalysisContextDataProvider : IContextDataProvider<CommitHistoryAnalysis>
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;


		public CommitAnalysisContextDataProvider(
			ICommitHistoryAnalyser commitHistoryAnalyser
		)
		{
			_commitHistoryAnalyser = commitHistoryAnalyser;
		}

		public CommitHistoryAnalysis LoadContextData()
		{
			return _commitHistoryAnalyser.Run();
		}
	}
}