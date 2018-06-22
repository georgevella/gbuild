using System.Collections.Generic;
using GBuild.Configuration.Models;

namespace GBuild.Configuration
{
	/// <summary>
	///		Processed data from configuration file.
	/// </summary>
	public interface IWorkspaceConfiguration
	{
		IEnumerable<IBranchVersioningStrategyModel> BranchVersioningStrategies { get; }
		SemanticVersion StartingVersion { get; }
		string SourceCodeRoot { get; }


	}
}