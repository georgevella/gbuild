using System.Collections.Generic;
using System.Linq;
using GBuild.Core.Configuration;

namespace GBuild.Core.Generator
{
    public interface IVersionNumberGeneratorProvider
    {
        IVersionNumberGenerator GetGenerator(BranchStrategy branchStrategy);
    }

    class VersionNumberGeneratorProvider : IVersionNumberGeneratorProvider
    {
        private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

        public VersionNumberGeneratorProvider(IEnumerable<IVersionNumberGenerator> versionNumberGenerators)
        {
            _versionNumberGenerators = versionNumberGenerators;
        }

        public IVersionNumberGenerator GetGenerator(BranchStrategy branchStrategy)
        {
            // TODO: implement mapping between branch strategy and version number generator
            return _versionNumberGenerators.First();
        }
    }
}