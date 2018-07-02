using System.Collections.Generic;
using GBuild.Models;

namespace GBuild.ReleaseHistory
{
	public interface IActiveReleasesProvider
	{
		IEnumerable<Release> GetActiveReleases();
	}
}