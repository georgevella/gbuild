using System.Collections.Generic;
using System.Linq;

namespace GBuild.Models
{
	public class Releases
	{
		public Releases(
			IEnumerable<Release> pastReleases,
			IEnumerable<Release> activeReleases
		)
		{
			ActiveReleases = activeReleases.ToList();
			PastReleases = pastReleases.ToList();
		}
		public IReadOnlyList<Release> PastReleases { get; }

		public IReadOnlyList<Release> ActiveReleases { get; }
	}
}