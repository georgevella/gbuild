using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;
using GBuild.Core.VcsSupport;

namespace GBuild.Core.Context.Providers
{
	public class BranchInformationContextDataProvider : IContextDataProvider<BranchInformation>
	{
		private readonly ConfigurationFile _configurationFile;
		private readonly ISourceCodeRepository _repository;

		public BranchInformationContextDataProvider(
			ISourceCodeRepository repository,
			ConfigurationFile configurationFile
		)
		{
			_repository = repository;
			_configurationFile = configurationFile;
		}

		public BranchInformation LoadContextData()
		{
			var branchVersioningStrategy =
				_configurationFile.Branches.FirstOrDefault(b => MatchesCurrentBranch(_repository.CurrentBranch, b.Name));

			return new BranchInformation(_repository.CurrentBranch, _repository.Branches, branchVersioningStrategy);
		}

		private bool MatchesCurrentBranch(
			Branch currentBranch,
			string filter
		)
		{
			if (currentBranch.Name == filter)
			{
				return true;
			}

			// TODO: pattern matching branch name
			return false;
		}
	}
}