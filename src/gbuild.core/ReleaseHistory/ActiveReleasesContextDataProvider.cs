using System.Linq;
using GBuild.Context;
using GBuild.Models;
using Microsoft.Extensions.Logging;

namespace GBuild.ReleaseHistory
{
	public class ActiveReleasesContextDataProvider : IContextDataProvider<ActiveReleases>
	{
		private readonly ILogger<ActiveReleasesContextDataProvider> _logger;
		private readonly IActiveReleasesProvider _activeReleasesProvider;

		public ActiveReleasesContextDataProvider(
			ILogger<ActiveReleasesContextDataProvider> logger,
			IActiveReleasesProvider activeReleasesProvider
			)
		{
			_logger = logger;
			_activeReleasesProvider = activeReleasesProvider;
		}
		public ActiveReleases LoadContextData()
		{
			_logger.LogInformation("Fetching active releases ...");
			var activeReleases = _activeReleasesProvider.GetActiveReleases().ToList();

			_logger.LogTrace("Fetching active releases ... complete");

			for (var index = 0; index < activeReleases.Count; index++)
			{
				var release = activeReleases[index];
				foreach (var p in release.VersionNumbers)
				{
					_logger.LogDebug("Release {index}: {project} - v{version}", index, p.Key.Name, p.Value);
				}
			}

			return new ActiveReleases(
				activeReleases
				);
		}
	}
}