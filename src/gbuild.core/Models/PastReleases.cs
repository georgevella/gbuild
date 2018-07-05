using System.Collections.Generic;
using System.Collections.ObjectModel;
using GBuild.Context;

namespace GBuild.Models
{
	public class PastReleases : ReadOnlyCollection<Release>, IContextEntity
	{
		public PastReleases(
			IList<Release> list
		)
			: base(list)
		{
		}
	}
}