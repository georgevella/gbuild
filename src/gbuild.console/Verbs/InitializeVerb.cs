using System.IO;
using GBuild.Console.VerbOptions;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using Serilog;

namespace GBuild.Console.Verbs
{
	public class InitializeVerb : IVerb<InitOptions>
	{
		private readonly ConfigurationFile _configurationFile;
		private readonly ILogger _log;
		private readonly IContextData<ProcessInformation> _processInformation;
		private readonly IContextData<RepositoryInformation> _sourceCodeInformation;

		public InitializeVerb(
			IContextData<ProcessInformation> processInformation,
			IContextData<RepositoryInformation> sourceCodeInformation,
			ConfigurationFile configurationFile
		)
		{
			_processInformation = processInformation;
			_sourceCodeInformation = sourceCodeInformation;
			_configurationFile = configurationFile;

			_log = Log.ForContext<InitializeVerb>();
		}

		public void Run(
			InitOptions options
		)
		{
			_log.Information("Current Directory: {currentDir}", _processInformation.Data.CurrentDirectory.FullName);
			_log.Information("Project Directory: {rootDir}",
							 _sourceCodeInformation.Data.RepositoryRootDirectory.FullName);

			var buildFilePath =
				Path.Combine(_sourceCodeInformation.Data.RepositoryRootDirectory.FullName, "build.yaml");
			_log.Information("Build File: {buildFilePath}", buildFilePath);

			var buildFile = new FileInfo(buildFilePath);
			if (buildFile.Exists && !options.Overwrite)
			{
				_log.Information(
					"build configuration already exists @ '{rootDir}'.  Use '--overwrite' option to replace.",
					_sourceCodeInformation.Data.RepositoryRootDirectory.FullName);
				return;
			}

			if (buildFile.Exists && !options.Overwrite)
			{
				_log.Information("build configuration @ '{rootDir}' will be overwritten with new settings.",
								 _sourceCodeInformation.Data.RepositoryRootDirectory.FullName);
			}

			using (var file = File.OpenWrite(buildFilePath))
			{
				ConfigurationWriter.Write(_configurationFile, file, true);
				file.Flush();
			}
		}
	}
}