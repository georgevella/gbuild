using System.Collections.Generic;
using GBuild.Configuration.Models;
using YamlDotNet.Serialization;

namespace GBuild.Configuration.Entities
{
	// TODO: configuration to specify how release tags are recognized and created
	public class ConfigurationFile
	{
		public static readonly ConfigurationFile Defaults = new ConfigurationFile
		{
			Branches =
			{
				KnownBranches = {
					new KnownBranchConfigurationModel
					{
						Name = "refs/heads/master",
						Type = BranchType.Main
					},
					new KnownBranchConfigurationModel
					{
						Name = "refs/heads/develop",
						Type = BranchType.Development,
						Analysis = {
							ParentBranch = "refs/heads/master",
						},
						Versioning =
						{
							Tag = "dev-{commitcount}"
						}
					},
					new KnownBranchConfigurationModel
					{
						Name = "refs/heads/feature/*",
						Type = BranchType.Feature,
						Analysis = {
							ParentBranch = "refs/heads/develop",
						},
						Versioning =
						{
							Tag = "dev-{featurename}-{commitcount}"
						}
					},
					new KnownBranchConfigurationModel
					{
						Name = "refs/heads/release/*",
						Type = BranchType.Release,
						Analysis = {
							ParentBranch = "refs/heads/develop",
							MergeTarget = "refs/heads/master"
						},
						Versioning =
						{
							Tag = "rc-{commitcount}"
						}
					}
				},
				BranchingModel = BranchingModelType.GitFlow
			},
			Sources = "src",
			Versioning = VersioningMode.Independent
		};
		public string StartingVersion { get; set; } = "0.1.0";

		/// <summary>
		///     Relative path to the location of all sources.
		/// </summary>
		public string Sources { get; set; }		
		public BranchConfigurationModel Branches { get; set; } = new BranchConfigurationModel();
		public VersioningMode Versioning { get; set; }
	}
}