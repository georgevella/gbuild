using System.Collections.Generic;
using System.Linq;
using gbuild.commitanalysis.git.Models;

namespace gbuild.commitanalysis.git.Extensions
{
	public static class BranchCollectionExtensions
	{
		public static IEnumerable<Branch> Convert(
			this IEnumerable<LibGit2Sharp.Branch> branchCollection
		)
		{
			return branchCollection.Select(_ => (Branch) _);
		}
	}
}