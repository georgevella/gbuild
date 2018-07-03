using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.Generator
{
	public interface IVersionNumberGeneratorProvider
	{
		WorkspaceVersionInfo GetVersion(CommitHistoryAnalysis commitHistoryAnalysis);

		WorkspaceVersionInfo GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis,
			IBranchVersioningSettings branchVersioningSettings
		);
	}
}