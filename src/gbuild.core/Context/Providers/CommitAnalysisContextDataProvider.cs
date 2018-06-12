using GBuild.CommitAnalysis;
using GBuild.Models;

namespace GBuild.Context.Providers
{
	public class CommitAnalysisContextDataProvider : IContextDataProvider<CommitAnalysisResult>
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;


		public CommitAnalysisContextDataProvider(
			ICommitHistoryAnalyser commitHistoryAnalyser
		)
		{
			_commitHistoryAnalyser = commitHistoryAnalyser;
		}

		public CommitAnalysisResult LoadContextData()
		{
			return _commitHistoryAnalyser.Run();
		}
	}
}