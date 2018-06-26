using System.Collections.Generic;
using GBuild.Models;

namespace GBuild.ReleaseHistory
{
	public interface IReleaseHistoryProvider
	{
		IEnumerable<Release> GetAllReleases();
		Release GetLatestRelease();
	}
}