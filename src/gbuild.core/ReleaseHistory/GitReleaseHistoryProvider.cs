using System.Collections.Generic;
using System.Linq;
using GBuild.Models;
using LibGit2Sharp;

namespace GBuild.ReleaseHistory
{
	public class GitReleaseHistoryProvider : IReleaseHistoryProvider
	{
		private readonly IRepository _repository;

		public GitReleaseHistoryProvider(IRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<Release> GetAllReleases()
		{
			return Enumerable.Empty<Release>();	
		}
	}
}