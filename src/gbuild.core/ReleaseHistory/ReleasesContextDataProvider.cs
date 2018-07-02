using GBuild.Context;
using GBuild.Models;
using GBuild.ReleaseHistory;

namespace GBuild.Workspaces
{
	public class WorkspaceReleaseInformationContextDataProvider : IContextDataProvider<Releases>
	{
		private readonly IReleaseHistoryProvider _releaseHistoryProvider;
		private readonly IActiveReleasesProvider _activeReleasesProvider;

		public WorkspaceReleaseInformationContextDataProvider(
			IReleaseHistoryProvider releaseHistoryProvider,
			IActiveReleasesProvider activeReleasesProvider
			)
		{
			_releaseHistoryProvider = releaseHistoryProvider;
			_activeReleasesProvider = activeReleasesProvider;
		}
		public Releases LoadContextData()
		{
			var releases = _releaseHistoryProvider.GetAllReleases();
//			var latestRelease = _releaseHistoryProvider.GetLatestRelease();
//			var latestReleaseVersionInfo = latestRelease?.VersionNumbers ?? WorkspaceVersionInfo.Empty();

			var activeReleases = _activeReleasesProvider.GetActiveReleases();

			// TODO: determine versions of any pending release branches, when in gitflow

			return new Releases(
				releases, 
				activeReleases
				);
		}
	}
}