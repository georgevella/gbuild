using GBuild.Console.VerbOptions;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Generator;
using Serilog;

namespace GBuild.Console.Verbs
{
	public class DescribeVerb : IVerb<DescribeOptions>
	{
		private readonly IContextData<CommitAnalysisResult> _commitAnalysis;
		private readonly IContextData<Workspace> _sourceCodeInformation;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;

		public DescribeVerb(
			IContextData<Workspace> sourceCodeInformation,
			IContextData<CommitAnalysisResult> commitAnalysis,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider
		)
		{
			_sourceCodeInformation = sourceCodeInformation;
			_commitAnalysis = commitAnalysis;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
		}

		public void Run(
			DescribeOptions options
		)
		{
			//Log.Information("Current Branch: {branch}", _commitAnalysis.Data.CurrentBranch);
			Log.Information("Current Directory: {repoRoot}",
							_sourceCodeInformation.Data.RepositoryRootDirectory.FullName);
			Log.Information("Current Directory: {srcRoot}",
							_sourceCodeInformation.Data.SourceCodeRootDirectory.FullName);
			Log.Information("Projects found: ");
			foreach (var module in _sourceCodeInformation.Data.Modules)
			{
				Log.Information($"+ {module.Name}");
			}

			Log.Information("Version: {version}", _versionNumberGeneratorProvider.GetVersion());

			Log.Information("Changed Modules: ");

			foreach (var changedModule in _commitAnalysis.Data.ChangedModules)
			{
				Log.Information($"+ {changedModule.Name}");
			}

			Log.Information("Commits:");
			foreach (var commit in _commitAnalysis.Data.Commits)
			{
				Log.Information($"{commit.Id}: {commit.Message}");
			}
		}
	}
}