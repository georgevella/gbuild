using System.Collections.Generic;
using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;

namespace GBuild.Core.Generator
{
    public interface IVersionNumberGeneratorProvider
    {
        SemanticVersion GetVersion();
    }

    internal class VersionNumberGeneratorProvider : IVersionNumberGeneratorProvider
    {
        private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;
        private readonly IContextData<BranchInformation> _branchInformation;
        private readonly ConfigurationFile _configuration;

        public VersionNumberGeneratorProvider(IEnumerable<IVersionNumberGenerator> versionNumberGenerators, IContextData<BranchInformation> branchInformation, ConfigurationFile configuration)
        {
            _versionNumberGenerators = versionNumberGenerators;
            _branchInformation = branchInformation;
            _configuration = configuration;
        }

        public SemanticVersion GetVersion()
        {
            var branchVersioningStrategy = _configuration.Branches.FirstOrDefault(b => MatchesCurrentBranch(b.Name));

            // TODO: implement mapping between branch strategy and version number generator
            var versionNumberGenerator = _versionNumberGenerators.First();

            return versionNumberGenerator.GetVersion(branchVersioningStrategy);
        }

        private bool MatchesCurrentBranch(string filter)
        {
            if (_branchInformation.Data.CurrentBranch.Name == filter)
                return true;

            // TODO: pattern matching branch name
            return false;
        }
    }
}