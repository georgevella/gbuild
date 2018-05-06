using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace GBuild.Core.Configuration
{
	public class ConfigurationFile
	{
		public static readonly ConfigurationFile Defaults = new ConfigurationFile
		{
			Branches =
			{
				new BranchVersioningStrategy
				{
					Name = "refs/heads/master"
				},
				new BranchVersioningStrategy
				{
					Name = "refs/heads/develop",
					ParentBranch = "refs/heads/master",
					Tag = "dev"
				},
				new BranchVersioningStrategy
				{
					Name = "refs/heads/feature/*",
					ParentBranch = "refs/heads/develop",
					Tag = "dev-{featurename}"
				}
			},
			SourceCodeRoot = "src",
			BranchingModel = BranchingModelType.GitFlow,
			IssueIdRegex = ""
		};

		/// <summary>
		///     Regex used to identify issue IDs in commits and branch names.
		/// </summary>
		[YamlMember(Alias = "issue-id-regex")]
		public string IssueIdRegex { get; set; }

		/// <summary>
		///     Relative path to the location of all sources.
		/// </summary>
		[YamlMember(Alias = "source-code-root")]
		public string SourceCodeRoot { get; set; }

		/// <summary>
		///     The branching model used in this repository.
		/// </summary>
		[YamlMember(Alias = "branching-model")]
		public BranchingModelType BranchingModel { get; set; }

		/// <summary>
		///     Branch specs.
		/// </summary>
		[YamlMember(Alias = "branches")]
		public List<BranchVersioningStrategy> Branches { get; set; } = new List<BranchVersioningStrategy>();
	}
}