using GBuild.Core.Context.Data;
using GBuild.Core.VcsSupport;

namespace GBuild.Core.Context.Providers
{
    public class GitBranchInformationContextDataProvider : IContextDataProvider<BranchInformation>
    {
        private readonly ISourceCodeRepository _repository;

        public GitBranchInformationContextDataProvider(ISourceCodeRepository repository)
        {
            _repository = repository;
        }

        public BranchInformation LoadContextData()
        {
            return new BranchInformation(_repository.CurrentBranch, _repository.Branches);
        }
    }
}