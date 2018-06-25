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
		private readonly IContextData<Workspace> _workspaceInformation;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;

		public DescribeVerb(
			IContextData<Workspace> workspaceInformation,
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
			Log.Information("Projects found: ");
			foreach (var module in _workspaceInformation.Data.Projects)
			{
				Log.Information($"+ {module.Name}");
			}			

			Log.Information("Changed Projects: ");

			foreach (var changedModule in _commitAnalysis.Data.ChangedProjects.Keys)
			{
				Log.Information($"+ {changedModule.Name}");
			}

			var workspaceVersionInfo = _versionNumberGeneratorProvider.GetVersion();

			Log.Information("Workspace Version Numbers:");

			foreach (var wvi in workspaceVersionInfo)
			{
				Log.Information($"+ {wvi.Key.Name} -> {wvi.Value}");
			}			

			//			Log.Information("Commits:");
			//			foreach (var commit in _commitAnalysis.Data.Commits)
			//			{
			//				Log.Information($"{commit.Id}: {commit.Message}");
			//			}
		}
	}
}