using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GBuild.Models
{
	public class ActiveReleases : ReadOnlyCollection<Release>
	{
		public ActiveReleases(
			IList<Release> list
		)
			: base(list)
		{
		}
	}
}