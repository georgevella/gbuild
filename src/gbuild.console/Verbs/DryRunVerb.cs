using System.Linq;
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
        private readonly IContextData<SourceCodeInformation> _sourceCodeInformation;
        private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;

        public DryRunVerb(IContextData<BranchInformation> branchInformation,
            IContextData<SourceCodeInformation> sourceCodeInformation,
            IVersionNumberGeneratorProvider versionNumberGeneratorProvider)
        {
            _branchInformation = branchInformation;
            _sourceCodeInformation = sourceCodeInformation;
            _versionNumberGeneratorProvider = versionNumberGeneratorProvider;
        }

        public void Run(DryRunOptions options)
        {
            Log.Information("Current Branch: {branch}", _branchInformation.Data.CurrentBranch);
            Log.Information("Current Directory: {repoRoot}",
                _sourceCodeInformation.Data.RepositoryRootDirectory.FullName);
            Log.Information("Current Directory: {srcRoot}",
                _sourceCodeInformation.Data.SourceCodeRootDirectory.FullName);
            Log.Information("Projects found: {@projects}", _sourceCodeInformation.Data.Projects.Count());
            Log.Information("Branches: {@branches}", _branchInformation.Data.Branches);

            Log.Information("Version: {version}", _versionNumberGeneratorProvider.GetVersion());
        }
    }
}