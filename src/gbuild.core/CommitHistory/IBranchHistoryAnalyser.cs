using System.Collections.Generic;
using GBuild.Models;

namespace GBuild.CommitHistory
{
	public interface IBranchHistoryAnalyser
	{
		IList<Commit> GetNewCommits();
	}
}