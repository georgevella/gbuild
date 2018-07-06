using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.CommitHistory;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace GBuild.ReleaseHistory
{
	internal class GitActiveReleasesProvider : IActiveReleasesProvider
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;
		private readonly ILogger<GitActiveReleasesProvider> _logger;
		private readonly IRepository _repository;
		private readonly IVersionNumberGeneratorProvider _versionNumberGenerator;
		private readonly IWorkspaceConfiguration _workspaceConfiguration;

		public GitActiveReleasesProvider(
			ILogger<GitActiveReleasesProvider> logger,
			IRepository repository,
			IWorkspaceConfiguration workspaceConfiguration,
			ICommitHistoryAnalyser commitHistoryAnalyser,
			IVersionNumberGeneratorProvider versionNumberGenerator
		)
		{
			_logger = logger;
			_repository = repository;
			_workspaceConfiguration = workspaceConfiguration;
			_commitHistoryAnalyser = commitHistoryAnalyser;
			_versionNumberGenerator = versionNumberGenerator;
		}

		public IEnumerable<Release> GetActiveReleases()
		{
			_logger.LogInformation("Fetching active releases ...");

			var releaseBranchType = _workspaceConfiguration.KnownBranches.FirstOrDefault(x => x.Type == BranchType.Release);
			var masterBranchType = _workspaceConfiguration.KnownBranches.FirstOrDefault(x => x.Type == BranchType.Main);

			if (releaseBranchType == null || masterBranchType == null)
			{
				throw new InvalidOperationException("There are no release or main type branches defined.");
			}

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

			// build active release history
			var result = releaseBranches.Select(branch =>
			{
				var releaseBranchCommitAnalysis = _commitHistoryAnalyser.AnalyseCommitLog(branch.CanonicalName);

				var releaseVersionInfo =
					_versionNumberGenerator.GetVersion(releaseBranchCommitAnalysis, releaseBranchType.VersioningSettings);

				return new Release(DateTime.Now, releaseVersionInfo);
			}).ToList();

			_logger.LogInformation("Found {activeReleaseCount} active releases", result.Count);

			return result;
		}
	}
}