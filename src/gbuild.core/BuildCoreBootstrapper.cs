﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using GBuild.Assemblies;
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
			container.RegisterSingleton<IContextDataStore, ContextDataStore>();
			container.RegisterSingleton<IContextDataLoader, ContextDataLoader>();
			container.RegisterInstance<ITypeLookupService>(new TypeLookupService(assemblies));

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
			container.RegisterSingleton<IConfigurationFileLoader, ConfigurationFileLoader>();
			container.RegisterSingleton<IWorkspaceConfiguration, WorkspaceConfiguration>();

			// workspace
			container.RegisterSingleton<IWorkspaceRootDirectoryProvider, WorkspaceRootDirectoryProvider>();
			container.RegisterSingleton<IWorkspaceSourceCodeDirectoryProvider, WorkspaceSourceCodeDirectoryProvider>();

			// version number generators
			container.RegisterCollection<IVersionNumberGenerator>(new []
			{
				Lifestyle.Transient.CreateRegistration<IndependentVersionNumberGenerator>(container)
			});
			container.RegisterCollection<IBranchVersioningStrategy>(new []
			{
				Lifestyle.Transient.CreateRegistration<DevelopmentBranchVersioningStrategy>(container),
				Lifestyle.Transient.CreateRegistration<ReleaseBranchVersioningStrategy>(container)
			});
			container.RegisterSingleton<IVersionNumberGeneratorProvider, VersionNumberGeneratorProvider>();
			container.RegisterSingleton<IVariableRenderer, VariableRenderer>();			
			container.RegisterSingleton<IBranchVersioningStrategyProvider, BranchVersioningStrategyProvider>();
			container.RegisterSingleton<IBranchHistoryAnalyserProvider, BranchHistoryAnalyserProvider>();
			container.RegisterCollection<IBranchHistoryAnalyser>(
				new []
				{
					Lifestyle.Singleton.CreateRegistration<DevelopmentBranchHistoryAnalyser>(container),
					Lifestyle.Singleton.CreateRegistration<ReleaseBranchHistoryAnalyser>(container)
				});
			
			container.RegisterInstance<IServiceProvider>(container);
		}
	}
}