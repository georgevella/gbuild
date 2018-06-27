using System.Linq;
using GBuild.Console.VerbOptions;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using Serilog;

namespace GBuild.Console.Verbs
{
	public class DescribeVerb : IVerb<DescribeOptions>
	{
		private readonly IContextData<CommitHistoryAnalysis> _commitAnalysis;
		private readonly IContextData<WorkspaceDescription> _workspaceInformation;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;

		public DescribeVerb(
			IContextData<WorkspaceDescription> workspaceInformation,
			IContextData<CommitHistoryAnalysis> commitAnalysis,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider
		)
		{
			_workspaceInformation = workspaceInformation;
			_commitAnalysis = commitAnalysis;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
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
			
			var currentVersions = _workspaceInformation.Data.ProjectLatestVersion;
			var nextVersions = _versionNumberGeneratorProvider.GetVersion();
			var longestProjectName = nextVersions.Keys.Select(x => x.Name.Length).Max();
			Log.Information("WorkspaceDescription Version Numbers:");

			foreach (var wvi in nextVersions)
			{
				var currrentVersion = currentVersions.ContainsKey(wvi.Key) ? currentVersions[wvi.Key].ToString() : "no-rel";
				Log.Information($"+ {wvi.Key.Name.PadLeft(longestProjectName)} : {currrentVersion} -> {wvi.Value}");
			}			

			Log.Information("Variables:");
			foreach (var pair in _workspaceInformation.Data.Variables)
			{
				Log.Information($"{pair.Key}: [{pair.Value}] (workspace)");
			}
		}
	}
}