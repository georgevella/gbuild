using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using GBuild.CommitHistory;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Projects.Discovery;
using GBuild.Projects.VersionWriter;
using GBuild.ReleaseHistory;
using GBuild.Variables;
using GBuild.Vcs;
using GBuild.Workspaces;
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

			container.RegisterCollection<IProjectVersionWriter>(assemblies);
			container.RegisterCollection<IProjectEnumerationService>(assemblies);
			container.RegisterSingleton<IProjectDiscoveryService, ProjectDiscoveryService>();

			// vcs support
			container.RegisterSingleton<IRepository, RepositoryWrapper>();
			container.RegisterSingleton<IReleaseHistoryProvider, GitReleaseHistoryProvider>();
			container.RegisterSingleton<ICommitHistoryAnalyser, GitCommitHistoryAnalyser>();
			container.RegisterSingleton<IGitRepositoryHelpers, GitRepositoryHelpers>();
			container.RegisterSingleton<IActiveReleasesProvider, GitActiveReleasesProvider>();

			// configuration
			container.RegisterSingleton<IConfigurationFile, ConfigurationFileLoaderService>();
			container.RegisterSingleton<IWorkspaceConfiguration, WorkspaceConfiguration>();

			// workspace
			container.RegisterSingleton<IWorkspaceRootDirectoryProvider, WorkspaceRootDirectoryProvider>();
			container.RegisterSingleton<IWorkspaceSourceCodeDirectoryProvider, WorkspaceSourceCodeDirectoryProvider>();

			// version number generators
			container.RegisterCollection<IVersionNumberGenerator>(assemblies);
			container.Register<IVersionNumberGeneratorProvider, VersionNumberGeneratorProvider>();
			container.Register<IVariableRenderer, VariableRenderer>();
			
			container.RegisterInstance<IServiceProvider>(container);
		}
	}
}