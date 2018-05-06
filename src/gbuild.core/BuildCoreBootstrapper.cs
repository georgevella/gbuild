using System;
using System.Collections.Generic;
using System.Reflection;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Generator;
using GBuild.Core.VcsSupport;
using GBuild.Core.VcsSupport.Git;
using SimpleInjector;

namespace GBuild.Core
{
    public static class BuildCoreBootstrapper
    {
        internal static IServiceProvider Start(ConfigurationFile configurationFile)
        {
            var container = new Container();
            BuildDependencyInjectionContainer(container, configurationFile);

            return container;
        }

        public static void BuildDependencyInjectionContainer(Container container, ConfigurationFile configurationFile)
        {
            IEnumerable<Assembly> assemblies = new[]
            {
                Assembly.GetExecutingAssembly()
            };

            // context information
            container.RegisterSingleton(typeof(IContextDataProvider<>), assemblies);
            container.RegisterSingleton(typeof(IContextData<>), typeof(ContextData<>));

            // vcs support
            container.RegisterSingleton<ISourceCodeRepository, GitSourceCodeRespository>();

            // configuration
            container.RegisterInstance(configurationFile);

            // version number generators
            container.RegisterCollection<IVersionNumberGenerator>(new[] {Assembly.GetExecutingAssembly()});
            container.Register<IVersionNumberGeneratorProvider, VersionNumberGeneratorProvider>();
        }
    }
}