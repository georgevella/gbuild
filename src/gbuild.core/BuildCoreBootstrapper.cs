using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.ReleaseHistory;
using GBuild.Workspace;
using LibGit2Sharp;
using SimpleInjector;

[assembly:InternalsVisibleTo("gbuild.tests")]

namespace GBuild
{
	public static class BuildCoreBootstrapper
	{
		public static void BuildDependencyInjectionContainer(
			Container container,
			Action<BuildCoreBootsrapperOptions> optionsFunc
		)
		{			
			var options = new BuildCoreBootsrapperOptions();

			optionsFunc(options);

			var assemblies = new List<Assembly>
			{
				Assembly.GetExecutingAssembly()
			};
			assemblies.AddRange(options.Assemblies);

			// context information
			container.RegisterSingleton(typeof(IContextDataProvider<>), assemblies);
			container.RegisterSingleton(typeof(IContextData<>), typeof(ContextData<>));

			// vcs support
			container.RegisterSingleton<IRepository, RepositoryWrapper>();

			// configuration
			container.RegisterSingleton<IConfigurationFile, ConfigurationFileLoaderService>();
			container.RegisterSingleton<IWorkspaceConfiguration, WorkspaceConfiguration>();

			// workspace
			container.RegisterSingleton<IWorkspaceRootDirectoryProvider, WorkspaceRootDirectoryProvider>();

			// version number generators
			container.RegisterCollection<IVersionNumberGenerator>(assemblies);
			container.Register<IVersionNumberGeneratorProvider, VersionNumberGeneratorProvider>();			
		}
	}
}