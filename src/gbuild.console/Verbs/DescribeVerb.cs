using System.Linq;
using GBuild.Console.VerbOptions;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using GBuild.Variables;
using Serilog;

namespace GBuild.Console.Verbs
{
	public class DescribeVerb : IVerb<DescribeOptions>
	{
		private readonly IContextData<CommitHistoryAnalysis> _commitAnalysis;
		private readonly IContextData<Workspace> _workspaceInformation;
		private readonly IContextData<PastReleases> _pastReleases;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;
		private readonly IVariableStore _variableStore;

		public DescribeVerb(
			IContextData<Workspace> workspaceInformation,
			IContextData<PastReleases> pastReleases,
			IContextData<CommitHistoryAnalysis> commitAnalysis,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider,
			IVariableStore variableStore
		)
		{
			_workspaceInformation = workspaceInformation;
			_pastReleases = pastReleases;
			_commitAnalysis = commitAnalysis;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
			_variableStore = variableStore;
		}

		public void Run(
			DescribeOptions options
		)
		{
			//Log.Information("Current Branch: {branch}", _commitAnalysis.Data.CurrentBranch);
			Log.Information("Current Directory: {repoRoot}",
							_workspaceInformation.Data.RepositoryRootDirectory.FullName);
			Log.Information("Current Directory: {srcRoot}",
							_workspaceInformation.Data.SourceCodeRootDirectory.FullName);

			Log.Information($"Projects found: {string.Join(",", _workspaceInformation.Data.Projects.Select( _ => _.Name))}");			
			Log.Information($"Changed Projects: {string.Join(",", _commitAnalysis.Data.ChangedProjects.Keys.Select(_ => _.Name))}");
			
			var currentVersions = _pastReleases.Data.FirstOrDefault()?.VersionNumbers ?? WorkspaceVersionInfo.Empty();
			var nextVersions = _versionNumberGeneratorProvider.GetVersion(_commitAnalysis.Data);
			var longestProjectName = nextVersions.Keys.Select(x => x.Name.Length).Max();
			Log.Information("Workspace Version Numbers:");

			foreach (var wvi in nextVersions)
			{
				var currrentVersion = currentVersions.ContainsKey(wvi.Key) ? currentVersions[wvi.Key].ToString() : "no-rel";
				Log.Information($"+ {wvi.Key.Name.PadLeft(longestProjectName)} : {currrentVersion} -> {wvi.Value}");
			}			

			Log.Information("Variables:");
			foreach (var pair in _variableStore.Global.GetVariables())
			{
				Log.Information($"{pair.Key}: [{pair.Value}] (workspace)");
			}
		}
	}
}