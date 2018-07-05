using System.Collections.Generic;
using System.Linq;
using GBuild.CommitHistory;
using GBuild.Console.VerbOptions;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using GBuild.Projects.VersionWriter;
using LibGit2Sharp;

namespace GBuild.Console.Verbs
{
	public class StampProjectsVerb : IVerb<StampProjectsOptions>
	{
		private readonly IContextData<Workspace> _workspaceContextData;
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;
		private readonly IRepository _repository;
		private readonly IVersionNumberGeneratorProvider _versionNumberGeneratorProvider;
		private readonly List<IProjectVersionWriter> _projectVersionWriters;

		public StampProjectsVerb(
			IContextData<Workspace> workspaceContextData,
			ICommitHistoryAnalyser commitHistoryAnalyser,
			IVersionNumberGeneratorProvider versionNumberGeneratorProvider,
			IEnumerable<IProjectVersionWriter> projectVersionWriters,
			IRepository repository
		)
		{
			_workspaceContextData = workspaceContextData;
			_commitHistoryAnalyser = commitHistoryAnalyser;
			_versionNumberGeneratorProvider = versionNumberGeneratorProvider;
			_repository = repository;
			_projectVersionWriters = projectVersionWriters.ToList();
		}

		/// <inheritdoc />
		public void Run(StampProjectsOptions options)
		{
			var currentBranch = _repository.GetCurrentBranch();
			var commitAnalysis = _commitHistoryAnalyser.AnalyseCommitLog(
				currentBranch.CanonicalName,
				_workspaceContextData.Data.BranchModel.AnalysisSettings
			);
			var changedProjects = commitAnalysis.ChangedProjects.Keys.ToList();
			var projectVersionWriter = _projectVersionWriters.First();
			var versionNumbers = _versionNumberGeneratorProvider.GetVersion(commitAnalysis);

			changedProjects.ForEach(project => projectVersionWriter.UpdateVersionInformation(project, versionNumbers[project]));
		}
	}
}