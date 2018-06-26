using GBuild.Models;

namespace GBuild.CommitHistory
{
	public interface ICommitHistoryAnalyser
	{
		CommitHistoryAnalysis Run();
	}
}