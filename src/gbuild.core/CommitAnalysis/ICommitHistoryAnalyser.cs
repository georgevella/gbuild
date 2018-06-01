using GBuild.Core.Context.Data;

namespace GBuild.Core.CommitAnalysis
{
	public interface ICommitHistoryAnalyser
	{
		CommitAnalysisResult Run();
	}
}