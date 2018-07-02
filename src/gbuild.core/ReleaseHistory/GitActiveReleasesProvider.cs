using System.Collections.Generic;
using System.Linq;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Models;
using LibGit2Sharp;

namespace GBuild.ReleaseHistory
{
	class GitActiveReleasesProvider : IActiveReleasesProvider
	{
		private readonly IRepository _repository;
		private readonly IWorkspaceConfiguration _workspaceConfiguration;

		public GitActiveReleasesProvider(
			IRepository repository, 
			IWorkspaceConfiguration workspaceConfiguration
			)
		{
			_repository = repository;
			_workspaceConfiguration = workspaceConfiguration;
		}
		public IEnumerable<Release> GetActiveReleases()
		{
			var releaseBranchType = _workspaceConfiguration.KnownBranches.FirstOrDefault(x => x.Type == BranchType.Release);
			var masterBranchType = _workspaceConfiguration.KnownBranches.FirstOrDefault(x => x.Type == BranchType.Main);

			var releaseBranches = _repository.Branches.Where(b => releaseBranchType.IsMatch(b.CanonicalName));
			var masterBranch = _repository.Branches.FirstOrDefault(b => masterBranchType.IsMatch(b.CanonicalName));

			// TODO: go through master branch history, either until original commit or until latest tag, and determine if any of these release branches are not merged.
			var commitFilter = new CommitFilter()
			{
				SortBy = CommitSortStrategies.Topological,
				IncludeReachableFrom = masterBranch,
				// ExcludeReachableFrom = Release Tag IF AVAILABLE
			};

			var masterCommits = _repository.Commits.QueryBy(commitFilter).ToList();

			// TODO: determine if a release branch was merged and left behind
			return Enumerable.Empty<Release>();
		}
	}
}