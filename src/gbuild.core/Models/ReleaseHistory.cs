using System;
using System.Collections.Generic;
using System.Linq;

namespace GBuild.Models
{
	public class ReleaseHistory
	{
		public ReleaseHistory(
			IEnumerable<Release> releases
		)
		{
			if (releases == null)
			{
				throw new ArgumentNullException(nameof(releases));
			}

			Releases = releases.ToList();
		}

		public IReadOnlyList<Release> Releases { get; }
	}
}