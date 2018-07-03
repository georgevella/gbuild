using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GBuild.Models
{
	public class PastReleases : ReadOnlyCollection<Release>
	{
		public PastReleases(
			IList<Release> list
		)
			: base(list)
		{
		}
	}
}