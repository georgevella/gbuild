using System.Collections.Generic;
using GBuild.Configuration.Models;
using YamlDotNet.Serialization;

namespace GBuild.Configuration.Entities
{
	public class BranchConfigurationModel
	{
		/// <summary>
		///     The branching model used in this repository.
		/// </summary>		
		public BranchingModelType BranchingModel { get; set; }

		/// <summary>
		///     Regex used to identify issue IDs in commits and branch names.
		/// </summary>
		public List<string> IssueTrackingPatterns { get; set; } = new List<string>();

		/// <summary>
		///     Branch specs.
		/// </summary>
		public List<KnownBranchConfigurationModel> KnownBranches { get; set; } = new List<KnownBranchConfigurationModel>();
	}
}