using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context;
using GBuild.Core.Generator;
using LibGit2Sharp;
using SimpleInjector;

[assembly:InternalsVisibleTo("gbuild.tests")]

namespace GBuild.Core
{
	public static class BuildCoreBootstrapper
	{
		public static void BuildDependencyInjectionContainer(
			Container container,
			ConfigurationFile configurationFile,
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
			container.RegisterInstance<IConfigurationFile>(configurationFile);
			container.RegisterSingleton<IWorkspaceConfiguration, WorkspaceConfiguration>();

			// version number generators
			container.RegisterCollection<IVersionNumberGenerator>(assemblies);
			container.Register<IVersionNumberGeneratorProvider, VersionNumberGeneratorProvider>();
		}
	}
}