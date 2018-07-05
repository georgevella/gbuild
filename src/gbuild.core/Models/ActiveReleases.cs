using System.Collections.Generic;
using System.Collections.ObjectModel;
using GBuild.Context;
using GBuild.Context.Attributes;

namespace GBuild.Models
{
	[DependsOnContextData(typeof(Workspace))]
	[DependsOnContextData(typeof(PastReleases))]
	public class ActiveReleases : ReadOnlyCollection<Release>, IContextEntity
	{
		public ActiveReleases(
			IList<Release> list
		)
			: base(list)
		{
		}
	}
}