using System;
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
		private readonly IContextData<CommitAnalysisResult> _commitAnalysis;
		private readonly IEnumerable<IVersionNumberGenerator> _versionNumberGenerators;

		public VersionNumberGeneratorProvider(
			IEnumerable<IVersionNumberGenerator> versionNumberGenerators,
			IContextData<CommitAnalysisResult> commitAnalysis,
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