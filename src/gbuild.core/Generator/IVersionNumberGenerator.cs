using System.Linq;
using GBuild.Core.VcsSupport;

namespace GBuild.Core.Generator
{
    public interface IVersionNumberGenerator
    {
        SemanticVersion GetVersion();
    }

    /// <summary>
    ///     Version number generator for a branch that is the 'development' counter part of an other branch designated as the
    ///     'production' branch.
    /// </summary>
    public class DevelopmentBranchVersionNumberGenerator : IVersionNumberGenerator
    {
        private readonly ISourceCodeRepository _sourceCodeRepository;

        public DevelopmentBranchVersionNumberGenerator(ISourceCodeRepository sourceCodeRepository)
        {
            _sourceCodeRepository = sourceCodeRepository;
        }

        public SemanticVersion GetVersion()
        {
            var commits = _sourceCodeRepository.GetCommitsBetween(
                _sourceCodeRepository.Branches.First(b => b.Name == "refs/heads/master"),
                _sourceCodeRepository.CurrentBranch
            );

            return SemanticVersion.Create(minor: 1, patch: 0, prereleseTag: $"dev-{commits.Count()}");
        }
    }
}