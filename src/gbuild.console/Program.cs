using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using GBuild.Core;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Vcs.Git;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using SimpleInjector;

namespace GBuild.Console
{
	internal class Program
	{
		private static void Main(
			string[] args
		)
		{
			// setup logging
			var configuration = new LoggerConfiguration()
				.WriteTo.Console(
					theme: AnsiConsoleTheme.Code,
					restrictedToMinimumLevel: LogEventLevel.Information
				);

			Log.Logger = configuration.CreateLogger();

			// configruation file
			var configurationFile = ConfigurationFile.Defaults;

			var repositoryRootDirectory = DetermineRepositoryRootDirectory();
			var buildYamlFile = repositoryRootDirectory.GetFiles("build.yaml", SearchOption.TopDirectoryOnly)
				.FirstOrDefault();

			if (buildYamlFile != null)
			{
				Log.Verbose("Configuration file found on disk at '{configFile}'", buildYamlFile.FullName);
				using (var file = buildYamlFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					configurationFile = ConfigurationFileReader.Read(file);
				}
			}
			else
			{
				Log.Verbose("Using configuration defaults");
			}

			// setup dependency injection container
			var container = new Container();

			BuildCoreBootstrapper.BuildDependencyInjectionContainer(
				container, 
				configurationFile,
				options => options.UseGit()
				);

			// register all verb runners
			var assemblyList = new List<Assembly>
			{
				Assembly.GetExecutingAssembly()
			};

			container.Register(typeof(IVerb<>), assemblyList);

			// setup command line parser
			var verbTypes = Assembly.GetExecutingAssembly().DefinedTypes
				.Select(t => new
				{
					TypeInfo = t,
					Type = t.AsType(),
					Verb = t.GetCustomAttribute<VerbAttribute>()
				})
				.Where(t => t.Verb != null).Select(t => t.Type)
				.ToArray();

			var parserResult = Parser.Default.ParseArguments(args, verbTypes);
			parserResult.WithParsed(o =>
			{
				var verbRunnerType = typeof(VerbRunner<>).MakeGenericType(o.GetType());
				var verbRunner = (IVerbRunner) container.GetInstance(verbRunnerType);

				verbRunner.Run(o);
			});

			parserResult.WithNotParsed(errors =>
			{
#if DEBUG
				foreach (var error in errors)
				{
					System.Console.WriteLine($"{error.Tag}: {error.GetType()}");
				}
#endif
			});

#if DEBUG
			System.Console.ReadKey();
#endif
		}

		private static DirectoryInfo DetermineRepositoryRootDirectory()
		{
			var repositoryRootDirectory = new DirectoryInfo(Environment.CurrentDirectory);
			var dotGitDirectory = new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
			while (!dotGitDirectory.Exists && repositoryRootDirectory.Parent != null)
			{
				repositoryRootDirectory = repositoryRootDirectory.Parent;
				dotGitDirectory = new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
			}

			if (dotGitDirectory.Exists)
			{
				return repositoryRootDirectory;
			}

			throw new InvalidOperationException("Cannot find git repository root.");
		}
	}
}