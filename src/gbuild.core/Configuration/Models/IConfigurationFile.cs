using System.Collections.Generic;

namespace GBuild.Configuration.Models
{
	public interface IConfigurationFile
	{
		string StartingVersion { get; set; }

		/// <summary>
		///     Regex used to identify issue IDs in commits and branch names.
		/// </summary>
		string IssueIdRegex { get; set; }

		/// <summary>
		///     Relative path to the location of all sources.
		/// </summary>
		string SourceCodeRoot { get; set; }

		/// <summary>
		///     The branching model used in this repository.
		/// </summary>
		BranchingModelType BranchingModel { get; set; }

		/// <summary>
		///     Branch specs.
		/// </summary>
		List<BranchVersioningStrategyModel> Branches { get; set; }
	}
}