using System.Collections.Generic;
using GBuild.Configuration.Models;

namespace GBuild.Configuration
{
	/// <summary>
	///     Processed data from configuration file.
	/// </summary>
	public interface IWorkspaceConfiguration
	{
		IEnumerable<IBranchVersioningStrategy> BranchVersioningStrategies { get; }

		IEnumerable<IKnownBranch> KnownBranches { get; }
		SemanticVersion StartingVersion { get; }
		string SourceCodeRoot { get; }
	}
}