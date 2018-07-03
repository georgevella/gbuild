using System.Collections.Generic;
using System.Linq;
using GBuild.Models;
using LibGit2Sharp;
using Commit = GBuild.Models.Commit;

namespace GBuild.CommitHistory
{
	public static class RepositoryExtensions
	{
		public static Branch GetCurrentBranch(
			this IRepository repository
		)
		{
			return repository.Branches.First(b => b.IsCurrentRepositoryHead);
		}
		public static Commit BuildCommitEntry(
			this IRepository repository,
			LibGit2Sharp.Commit arg
		)
		{
			// TODO: store merge commit parental history
			var treeChanges = new List<TreeEntryChanges>();

			foreach (var parent in arg.Parents)
			{
				treeChanges.AddRange(
					repository.Diff.Compare<TreeChanges>(parent.Tree, arg.Tree)
				);
			}

			var changedFiles = treeChanges.Select(e => new ChangedFile(e.Path)).ToList();

			return new Commit(arg.Id.Sha, arg.Committer.Name, arg.Message, changedFiles);
		}
	}
}