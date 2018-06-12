using System;
using System.Collections.Generic;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Generator
{
	internal class VersionNumberGeneratorProvider : IVersionNumberGeneratorProvider
	{
		private readonly IContextData<CommitHistoryAnalysis> _commitAnalysis;
		private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

		public VersionNumberGeneratorProvider(
			IEnumerable<IVersionNumberGenerator> versionNumberGenerators,
			IContextData<CommitHistoryAnalysis> commitAnalysis,
			ConfigurationFile configuration
		)
		{
			_versionNumberGenerators = versionNumberGenerators;
			_commitAnalysis = commitAnalysis;
		}

		public WorkspaceVersionNumbers GetVersion()
		{
			// TODO: implement mapping between branch strategy and version number generator
//			var versionNumberGenerator = _versionNumberGenerators.First();
//
//			return versionNumberGenerator.GetVersion(_commitAnalysis.Data.VersioningStrategyModel);

			throw new NotImplementedException();
		}
	}
}