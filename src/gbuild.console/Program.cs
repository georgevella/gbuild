using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using GBuild.CommitHistory;
using GBuild.Configuration.IO;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.ReleaseHistory;
using GBuild.Vcs;
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
				.MinimumLevel.Verbose()
				.WriteTo.Console(
					theme: AnsiConsoleTheme.Code,
					restrictedToMinimumLevel: LogEventLevel.Verbose
				);

			Log.Logger = configuration.CreateLogger();			

			// setup dependency injection container
			var container = new Container();

			BuildCoreBootstrapper.BuildDependencyInjectionContainer(
				container, 
				options =>
				{
					
				}
				);

			// register all verb runners
			var assemblyList = new List<Assembly>
			{
				Assembly.GetExecutingAssembly()
			};

			container.Register(typeof(IVerb<>), assemblyList);

			container.Verify(VerificationOption.VerifyAndDiagnose);

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
				var contextDataLoader = container.GetInstance<IContextDataLoader>();
				contextDataLoader.PrepareContextData();

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
	}
}