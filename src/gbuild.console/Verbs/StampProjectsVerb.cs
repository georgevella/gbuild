using System.Collections.Generic;
using System.Linq;
using GBuild.Console.VerbOptions;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using GBuild.Projects.VersionWriter;

namespace GBuild.Console.Verbs
{
	public class StampProjectsVerb : IVerb<StampProjectsOptions>
	{
		private readonly IContextData<Workspace> _workspaceInformation;
		private readonly IContextData<CommitHistoryAnalysis> _commitAnalysis;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;
		private readonly List<IProjectVersionWriter> _projectVersionWriters;

		public StampProjectsVerb(
			IContextData<Workspace> workspaceInformation,
			IContextData<CommitHistoryAnalysis> commitAnalysis,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider,
			IEnumerable<IProjectVersionWriter> projectVersionWriters
			)
		{
			_workspaceInformation = workspaceInformation;
			_commitAnalysis = commitAnalysis;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
			_projectVersionWriters = projectVersionWriters.ToList();
		}

		/// <inheritdoc />
		public void Run(StampProjectsOptions options)
		{
			var changedProjects = _commitAnalysis.Data.ChangedProjects.Keys.ToList();
			var projectVersionWriter = _projectVersionWriters.First();
			var versionNumbers = _versionNumberGeneratorProvider.GetVersion(_commitAnalysis.Data);

			changedProjects.ForEach(project => projectVersionWriter.UpdateVersionInformation(project, versionNumbers[project]));
		}
	}
}