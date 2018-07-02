using System.Collections.Generic;
using LibGit2Sharp;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	class ReleaseBranchHistoryAnalyser : IBranchHistoryAnalyser
	{
		private readonly IRepository _repository;

		public ReleaseBranchHistoryAnalyser(
			IRepository repository
		)
		{
			_repository = repository;
		}

		public IList<Commit> GetNewCommits()
		{
			throw new System.NotImplementedException();
		}
	}
}