using System.Collections.Generic;
using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;

namespace GBuild.Core.Generator
{
	internal class VersionNumberGeneratorProvider : IVersionNumberGeneratorProvider
	{
		private readonly IContextData<VersionControl> _branchInformation;
		private readonly ConfigurationFile _configuration;
		private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

		public VersionNumberGeneratorProvider(
			IEnumerable<IVersionNumberGenerator> versionNumberGenerators,
			IContextData<VersionControl> branchInformation,
			ConfigurationFile configuration
		)
		{
			_versionNumberGenerators = versionNumberGenerators;
			_branchInformation = branchInformation;
			_configuration = configuration;
		}

		public SemanticVersion GetVersion()
		{
			// TODO: implement mapping between branch strategy and version number generator
			var versionNumberGenerator = _versionNumberGenerators.First();

			return versionNumberGenerator.GetVersion(_branchInformation.Data.VersioningStrategyModel);
		}
	}
}