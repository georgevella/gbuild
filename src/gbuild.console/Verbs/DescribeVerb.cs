using System.Linq;
using GBuild.CommitHistory;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using GBuild.Variables;
using LibGit2Sharp;
using Serilog;
using DescribeOptions = GBuild.Console.VerbOptions.DescribeOptions;

namespace GBuild.Console.Verbs
{
	public class DescribeVerb : IVerb<DescribeOptions>
	{
		private readonly IContextData<Workspace> _workspaceContextData;
		private readonly IContextData<PastReleases> _pastReleases;
		private readonly IRepository _repository;
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;
		private readonly IBranchHistoryAnalyser _branchHistoryAnalyser;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;

		public DescribeVerb(
			IContextData<Workspace> workspaceContextData,
			IContextData<PastReleases> pastReleases,
			IRepository repository,
			ICommitHistoryAnalyser commitHistoryAnalyser,
			IBranchHistoryAnalyser branchHistoryAnalyser,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider
		)
		{
			_workspaceContextData = workspaceContextData;
			_pastReleases = pastReleases;
			_repository = repository;
			_commitHistoryAnalyser = commitHistoryAnalyser;
			_branchHistoryAnalyser = branchHistoryAnalyser;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
		}

		public void Run(
			DescribeOptions options
		)
		{
			var currentBranch = _repository.GetCurrentBranch();

			var commitHistoryAnalysis = _commitHistoryAnalyser.AnalyseCommitLog(
				_branchHistoryAnalyser,
				_workspaceContextData.Data.BranchModel.AnalysisSettings,
				currentBranch.CanonicalName);

			//Log.Information("Current Branch: {branch}", _commitAnalysis.Data.CurrentBranch);
			Log.Information("Current Directory: {repoRoot}",
							_workspaceContextData.Data.RepositoryRootDirectory.FullName);
			Log.Information("Current Directory: {srcRoot}",
							_workspaceContextData.Data.SourceCodeRootDirectory.FullName);

			Log.Information($"Projects found: {string.Join(",", _workspaceContextData.Data.Projects.Select( _ => _.Name))}");			
			Log.Information($"Changed Projects: {string.Join(",", commitHistoryAnalysis.ChangedProjects.Keys.Select(_ => _.Name))}");
			
			var currentVersions = _pastReleases.Data.FirstOrDefault()?.VersionNumbers ?? WorkspaceVersionInfo.Empty();
			var nextVersions = _versionNumberGeneratorProvider.GetVersion(commitHistoryAnalysis);
			var longestProjectName = nextVersions.Keys.Select(x => x.Name.Length).Max();
			Log.Information("Workspace Version Numbers:");

			foreach (var wvi in nextVersions)
			{
				var currrentVersion = currentVersions.ContainsKey(wvi.Key) ? currentVersions[wvi.Key].ToString() : "no-rel";
				Log.Information($"+ {wvi.Key.Name.PadLeft(longestProjectName)} : {currrentVersion} -> {wvi.Value}");
			}			

//			Log.Information("Variables:");
//			foreach (var pair in _variableStore.Global.GetVariables())
//			{
//				Log.Information($"{pair.Key}: [{pair.Value}] (workspace)");
//			}
		}
	}
}