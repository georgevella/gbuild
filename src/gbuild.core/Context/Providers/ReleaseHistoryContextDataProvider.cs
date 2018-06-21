using System.Linq;
using GBuild.ReleaseHistory;

namespace GBuild.Context.Providers
{
	public class ReleaseHistoryContextDataProvider : IContextDataProvider<Models.ReleaseHistory>
	{
		private readonly IReleaseHistoryProvider _releaseHistoryProvider;

		public ReleaseHistoryContextDataProvider(IReleaseHistoryProvider releaseHistoryProvider)
		{
			_releaseHistoryProvider = releaseHistoryProvider;
		}

		public Models.ReleaseHistory LoadContextData()
		{
			var releases = _releaseHistoryProvider.GetAllReleases();

			return new Models.ReleaseHistory(releases.ToList());
		}
	}
}