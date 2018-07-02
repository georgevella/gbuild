using System.Collections.Generic;
using System.Linq;
using GBuild.Models;

namespace GBuild.ReleaseHistory
{
	public interface IActiveReleasesProvider
	{
		IEnumerable<Release> GetActiveReleases();
	}

	class GitActiveReleasesProvider : IActiveReleasesProvider
	{
		public IEnumerable<Release> GetActiveReleases()
		{
			return Enumerable.Empty<Release>();
		}
	}
}