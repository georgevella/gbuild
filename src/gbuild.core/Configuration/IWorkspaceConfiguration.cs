using System.Collections.Generic;
using GBuild.Core.Configuration.Models;

namespace GBuild.Core.Configuration
{
	/// <summary>
	///		Processed data from configuration file.
	/// </summary>
	public interface IWorkspaceConfiguration
	{
		IEnumerable<IBranchVersioningStrategyModel> BranchVersioningStrategies { get; }
		SemanticVersion StartingVersion { get; }
	}
}