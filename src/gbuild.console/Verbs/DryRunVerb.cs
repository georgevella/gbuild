using GBuild.Console.VerbOptions;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Generator;
using Serilog;

namespace GBuild.Console.Verbs
{
	public class DryRunVerb : IVerb<DryRunOptions>
	{
		private readonly IContextData<BranchInformation> _branchInformation;
		private readonly IContextData<CommitAnalysis> _commitAnalysis;
		private readonly IContextData<RepositoryInformation> _sourceCodeInformation;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;

		public DryRunVerb(
			IContextData<BranchInformation> branchInformation,
			IContextData<RepositoryInformation> sourceCodeInformation,
			IContextData<CommitAnalysis> commitAnalysis,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider
		)
		{
			_branchInformation = branchInformation;
			_sourceCodeInformation = sourceCodeInformation;
			_commitAnalysis = commitAnalysis;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
		}

		public void Run(
			DryRunOptions options
		)
		{
			Log.Information("Current Branch: {branch}", _branchInformation.Data.CurrentBranch);
			Log.Information("Current Directory: {repoRoot}",
							_sourceCodeInformation.Data.RepositoryRootDirectory.FullName);
			Log.Information("Current Directory: {srcRoot}",
							_sourceCodeInformation.Data.SourceCodeRootDirectory.FullName);
			Log.Information("Projects found: ");
			foreach (var module in _sourceCodeInformation.Data.Modules)
			{
				Log.Information($"+ {module.Name}");
			}

			Log.Information("Branches: {@branches}", _branchInformation.Data.Branches);

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