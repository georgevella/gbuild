using System.Linq;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.VcsSupport;

namespace GBuild.Core.Generator
{
	/// <summary>
	///     Version number generator for a branch that is the 'development' counter part of an other branch designated as the
	///     'production' branch.
	/// </summary>
	public class DevelopmentBranchVersionNumberGenerator : IVersionNumberGenerator
	{
		private readonly ConfigurationFile _configurationFile;
		private readonly IContextData<CommitAnalysis> _commitAnalysis;

		public DevelopmentBranchVersionNumberGenerator(
			ConfigurationFile configurationFile,
			IContextData<CommitAnalysis> commitAnalysis
		)
		{
			_configurationFile = configurationFile;
			_commitAnalysis = commitAnalysis;
		}

		public SemanticVersion GetVersion(
			BranchVersioningStrategy branchVersioningStrategy
		)
		{
			var startingVersion = SemanticVersion.Parse(_configurationFile.StartingVersion);

			return SemanticVersion.Create(
				major: startingVersion.Major,
				minor: startingVersion.Minor, 
				patch: startingVersion.Patch, 
				prereleseTag: $"{branchVersioningStrategy.Tag}-{_commitAnalysis.Data.Commits.Count()}"
				);
		}
	}
}