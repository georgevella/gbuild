using System.Collections.Generic;
using System.Linq;
using GBuild.Models;
using GBuild.ReleaseHistory;
using LibGit2Sharp;

namespace gbuild.commitanalysis.git
{
	public class ReleaseHistoryProvider : IReleaseHistoryProvider
	{
		private readonly IRepository _repository;

		public ReleaseHistoryProvider(IRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<Release> GetAllReleases()
		{
			return Enumerable.Empty<Release>();	
		}
	}
}