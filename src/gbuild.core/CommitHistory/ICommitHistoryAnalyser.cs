using GBuild.Models;

namespace GBuild.CommitHistoryAnalyser
{
	public interface ICommitHistoryAnalyser
	{
		CommitHistoryAnalysis Run();
	}
}