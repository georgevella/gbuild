using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;

namespace GBuild.Core.Generator
{
	/// <summary>
	///     Version number generator for a branch that is the 'development' counter part of an other branch designated as the
	///     'production' branch.
	/// </summary>
	public class DevelopmentBranchVersionNumberGenerator : IVersionNumberGenerator
	{
		private readonly IConfigurationFile _configurationFile;
		private readonly IContextData<CommitAnalysisResult> _commitAnalysis;

		public DevelopmentBranchVersionNumberGenerator(
			IConfigurationFile configurationFile,
			IContextData<CommitAnalysisResult> commitAnalysis
		)
		{
			_configurationFile = configurationFile;
			_commitAnalysis = commitAnalysis;
		}

		public SemanticVersion GetVersion(
			IBranchVersioningStrategyModel branchVersioningStrategyModel
		)
		{
			var startingVersion = SemanticVersion.Parse(_configurationFile.StartingVersion);

			return SemanticVersion.Create(
				major: startingVersion.Major,
				minor: startingVersion.Minor, 
				patch: startingVersion.Patch, 
				prereleseTag: $"{branchVersioningStrategyModel.Tag}-{_commitAnalysis.Data.NumberOfChanges}",
				metadata: branchVersioningStrategyModel.Metadata
				);
		}
	}
}