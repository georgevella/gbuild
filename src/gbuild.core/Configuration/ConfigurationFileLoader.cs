using System.Collections.Generic;
using System.IO;
using System.Linq;
using GBuild.Configuration.Entities;
using GBuild.Configuration.IO;
using GBuild.Configuration.Models;
using GBuild.Workspaces;
using Serilog;

namespace GBuild.Configuration
{
	public class ConfigurationFileLoader : IConfigurationFileLoader
	{
		private readonly IWorkspaceRootDirectoryProvider _workspaceRootDirectoryProvider;

		public ConfigurationFileLoader(IWorkspaceRootDirectoryProvider workspaceRootDirectoryProvider)
		{
			_workspaceRootDirectoryProvider = workspaceRootDirectoryProvider;
		}

		public ConfigurationFile Load()
		{
			// TODO: configuration file sanitization
			// 1 main branch type
			// 1 release branch type (do we need this restriction?)
			// 1 development branch type (do we need this restriction?)
			var workspaceRootDirectory = _workspaceRootDirectoryProvider.GetWorkspaceRootDirectory();
			var buildYamlFile = workspaceRootDirectory.GetFiles("build.yaml", SearchOption.TopDirectoryOnly)
				.FirstOrDefault();

			if (buildYamlFile != null)
			{
				Log.Verbose("Configuration file found on disk at '{configFile}'", buildYamlFile.FullName);
				using (var file = buildYamlFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					return ConfigurationFileReader.Read(file);
				}
			}
			else
			{
				Log.Verbose("Using configuration defaults");
				return ConfigurationFile.Defaults;
			}
		}
	}
}