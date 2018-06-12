using GBuild.Models;

namespace GBuild.CommitAnalysis
{
	public interface ICommitHistoryAnalyser
	{
		CommitAnalysisResult Run();
	}
}