using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.CommitHistory;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using GBuild.Variables;
using Humanizer;
using LibGit2Sharp;

namespace GBuild.Generator
{
	internal class VersionNumberGeneratorProvider : IVersionNumberGeneratorProvider
	{
		private readonly IContextData<Workspace> _workspaceContextData;
		private readonly IRepository _repository;
		private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

		public VersionNumberGeneratorProvider(
			IEnumerable<IVersionNumberGenerator> versionNumberGenerators,
			IContextData<Workspace> workspaceContextData,			
			IRepository repository
		)
		{
			_versionNumberGenerators = versionNumberGenerators;
			_workspaceContextData = workspaceContextData;
			_repository = repository;
		}

		public WorkspaceVersionInfo GetVersion(CommitHistoryAnalysis commitHistoryAnalysis)
		{
			var variableStore = BuildVariableStore(commitHistoryAnalysis);

			// TODO: implement mapping between branch strategy and version number generator
			var versionNumberGenerator = _versionNumberGenerators.First();

			var workspaceVersionInfo = new Dictionary<Project, SemanticVersion>();

			foreach (var project in _workspaceContextData.Data.Projects)
			{
				var projectVersion = versionNumberGenerator.GetVersion(
					commitHistoryAnalysis,
					project,
					variableStore
					);
				workspaceVersionInfo[project] = projectVersion;
			}

			return new WorkspaceVersionInfo(workspaceVersionInfo);
		}
		public WorkspaceVersionInfo GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis,
			IBranchVersioningSettings branchVersioningSettings
		)
		{
			var variableStore = BuildVariableStore(commitHistoryAnalysis);
			var versionNumberGenerator = _versionNumberGenerators.First();

			var workspaceVersionInfo = new Dictionary<Project, SemanticVersion>();

			foreach (var project in _workspaceContextData.Data.Projects)
			{
				var projectVersion = versionNumberGenerator.GetVersion(commitHistoryAnalysis, project, variableStore);
				workspaceVersionInfo[project] = projectVersion;
			}

			return new WorkspaceVersionInfo(workspaceVersionInfo);
		}

		private VariableStore BuildVariableStore(CommitHistoryAnalysis commitHistoryAnalysis)
		{
			var variableStore = new VariableStore();

			_workspaceContextData.Data.Projects.ToList().ForEach(p => variableStore.AddProject(p));

			foreach (var pair in commitHistoryAnalysis.ChangedProjects)
			{
				variableStore.ProjectVariables[pair.Key][ProjectVariables.CommitCount] =
					pair.Value.CommitsAheadOfParent.Count().ToString();
			}

			foreach (var pair in BuildWorkspaceVariables(_repository.Branches.First(x => x.CanonicalName == commitHistoryAnalysis.BranchName)))
			{
				variableStore.Global[pair.Key] = pair.Value;
			}

			return variableStore;
		}

		private IDictionary<string, string> BuildWorkspaceVariables(Branch currentBranch)
		{
			return new Dictionary<string, string>()
			{
				{WorkspaceVariables.BranchName, currentBranch.FriendlyName},
				{WorkspaceVariables.FeatureName, GetFeatureNameFromBranch(currentBranch)},
				{WorkspaceVariables.IssueId, GetIssueIdFromBranch(currentBranch)}
			};
		}

		private string GetIssueIdFromBranch(
			Branch currentBranch
		)
		{
			// TODO: add support
			return String.Empty;
		}

		private string GetFeatureNameFromBranch(
			Branch currentBranch
		)
		{
			if (currentBranch.CanonicalName.StartsWith("refs/heads/feature/"))
			{
				var featureName = currentBranch.CanonicalName.Substring("refs/heads/feature/".Length);
				featureName = new String(featureName.Where(c => c != '-' && c != '_').ToArray()).Transform(To.LowerCase)
					.Truncate(10, string.Empty);
				return featureName;
			}

			return string.Empty;
		}
	}
}