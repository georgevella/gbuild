using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;
using GBuild.Core.Vcs;

namespace GBuild.Core.Context.Providers
{
	public class VersionControlContextDataProvider : IContextDataProvider<VersionControl>
	{
		private readonly ConfigurationFile _configurationFile;
		private readonly ISourceCodeRepository _repository;

		public VersionControlContextDataProvider(
			ISourceCodeRepository repository,
			ConfigurationFile configurationFile
		)
		{
			_repository = repository;
			_configurationFile = configurationFile;
		}

		public VersionControl LoadContextData()
		{
			var branchVersioningStrategy =
				_configurationFile.Branches.FirstOrDefault(b => MatchesCurrentBranch(_repository.CurrentBranch, b.Name));

			return new VersionControl(_repository.CurrentBranch, branchVersioningStrategy);
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