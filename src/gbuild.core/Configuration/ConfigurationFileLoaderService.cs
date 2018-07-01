using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBuild.Configuration.IO;
using GBuild.Configuration.Models;
using GBuild.Workspaces;
using Serilog;

namespace GBuild.Configuration
{
	public class ConfigurationFileLoaderService : IConfigurationFile
	{
		private readonly IConfigurationFile _conf;

		public ConfigurationFileLoaderService(IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider)
		{
			var workspaceRootDirectory = workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();
			var buildYamlFile = workspaceRootDirectory.GetFiles("build.yaml", SearchOption.TopDirectoryOnly)
				.FirstOrDefault();

			if (buildYamlFile != null)
			{
				Log.Verbose("Configuration file found on disk at '{configFile}'", buildYamlFile.FullName);
				using (var file = buildYamlFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					_conf = ConfigurationFileReader.Read(file);
				}
			}
			else
			{
				Log.Verbose("Using configuration defaults");
				_conf = ConfigurationFile.Defaults;
			}
		}

		public string StartingVersion
		{
			get => _conf.StartingVersion;
			set => _conf.StartingVersion = value;
		}

		public string IssueIdRegex
		{
			get => _conf.IssueIdRegex;
			set => _conf.IssueIdRegex = value;
		}

		public string SourceCodeRoot
		{
			get => _conf.SourceCodeRoot;
			set => _conf.SourceCodeRoot = value;
		}

		public BranchingModelType BranchingModel
		{
			get => _conf.BranchingModel;
			set => _conf.BranchingModel = value;
		}

		public List<BranchVersioningStrategyModel> Branches
		{
			get => _conf.Branches;
			set => _conf.Branches = value;
		}
	}
}