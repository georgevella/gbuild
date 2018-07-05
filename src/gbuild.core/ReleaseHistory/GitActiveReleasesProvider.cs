using System.Collections.Generic;
using System.Linq;
using GBuild.CommitHistory;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using LibGit2Sharp;

namespace GBuild.ReleaseHistory
{
	internal class GitActiveReleasesProvider : IActiveReleasesProvider
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;
		private readonly IRepository _repository;
		private readonly IVersionNumberGeneratorProvider _versionNumberGenerator;
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly IContextData<Workspace> _workspaceContextData;

		public GitActiveReleasesProvider(
			IRepository repository,
			IWorkspaceConfiguration workspaceConfiguration,
			ICommitHistoryAnalyser commitHistoryAnalyser,
			IContextData<Workspace> workspaceContextData,
			IVersionNumberGeneratorProvider versionNumberGenerator
		)
		{
			_repository = repository;
			_workspaceConfiguration = workspaceConfiguration;
			_commitHistoryAnalyser = commitHistoryAnalyser;
			_workspaceContextData = workspaceContextData;
			_versionNumberGenerator = versionNumberGenerator;
		}

		public IEnumerable<Release> GetActiveReleases()
		{
			var releaseBranchType = _workspaceConfiguration.KnownBranches.FirstOrDefault(x => x.Type == BranchType.Release);
			var masterBranchType = _workspaceConfiguration.KnownBranches.FirstOrDefault(x => x.Type == BranchType.Main);

			var releaseBranches = _repository.Branches.Where(b => releaseBranchType.IsMatch(b.CanonicalName));
			var masterBranch = _repository.Branches.FirstOrDefault(b => masterBranchType.IsMatch(b.CanonicalName));

			// TODO: go through master branch history, either until original commit or until latest tag, and determine if any of these release branches are not merged.
			var commitFilter = new CommitFilter
			{
				SortBy = CommitSortStrategies.Topological,
				IncludeReachableFrom = masterBranch
				// ExcludeReachableFrom = Release Tag IF AVAILABLE
			};

			var masterCommits = _repository.Commits.QueryBy(commitFilter).ToList();

			// TODO: determine if a release branch was merged and left behind

			var releaseBranchCommitAnalysis = _commitHistoryAnalyser.AnalyseCommitLog(releaseBranches.First().CanonicalName, releaseBranchType.AnalysisSettings);

			var releaseVersionInfo = _versionNumberGenerator.GetVersion(releaseBranchCommitAnalysis, releaseBranchType.VersioningSettings);


			return Enumerable.Empty<Release>();
		}
	}
}