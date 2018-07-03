using System.Linq;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.ReleaseHistory
{
	public class PastReleasesContextDataProvider : IContextDataProvider<PastReleases>
	{
		private readonly IReleaseHistoryProvider _releaseHistoryProvider;

		public PastReleasesContextDataProvider(
			IReleaseHistoryProvider releaseHistoryProvider
		)
		{
			_releaseHistoryProvider = releaseHistoryProvider;
		}
		public PastReleases LoadContextData()
		{
			var releases = _releaseHistoryProvider.GetAllReleases();			
			return new PastReleases(releases.ToList());
		}
	}
}