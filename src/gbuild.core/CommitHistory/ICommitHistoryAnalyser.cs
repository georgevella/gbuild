using System.Collections.Generic;
using System.IO;
using GBuild.Configuration.Models;
using GBuild.Models;

namespace GBuild.CommitHistory
{
	public interface ICommitHistoryAnalyser
	{
		CommitHistoryAnalysis AnalyseCommitLog(IBranchVersioningStrategyModel branchVersioningStrategy, DirectoryInfo repositoryRootDirectory, IEnumerable<Project> projects);
	}
}