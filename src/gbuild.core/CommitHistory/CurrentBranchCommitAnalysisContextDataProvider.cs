using System.Linq;
using GBuild.Context;
using GBuild.Models;
using GBuild.Variables;
using LibGit2Sharp;

namespace GBuild.CommitHistory
{
	public class CurrentBranchCommitAnalysisContextDataProvider : IContextDataProvider<CommitHistoryAnalysis>
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;
		private readonly IBranchHistoryAnalyser _branchHistoryAnalyser;
		private readonly IContextData<Workspace> _workspaceContextData;
		private readonly IVariableStore _variableStore;
		private readonly IRepository _repository;

		public CurrentBranchCommitAnalysisContextDataProvider(
			ICommitHistoryAnalyser commitHistoryAnalyser,
			IBranchHistoryAnalyser branchHistoryAnalyser,
			IContextData<Workspace> workspaceContextData,
			IVariableStore variableStore,
			IRepository repository
		)
		{
			_commitHistoryAnalyser = commitHistoryAnalyser;
			_branchHistoryAnalyser = branchHistoryAnalyser;
			_workspaceContextData = workspaceContextData;
			_variableStore = variableStore;
			_repository = repository;
		}
		public CommitHistoryAnalysis LoadContextData()
		{
			var currentBranch = _repository.GetCurrentBranch();

			var commitHistoryAnalysis = _commitHistoryAnalyser.AnalyseCommitLog(
				_branchHistoryAnalyser,
				_workspaceContextData.Data.BranchModel.AnalysisSettings,
				currentBranch.CanonicalName,
				_workspaceContextData.Data.RepositoryRootDirectory, 
				_workspaceContextData.Data.Projects);

			// TODO: this should be placed in it's own little place
			foreach (var pair in commitHistoryAnalysis.ChangedProjects)
			{
				_variableStore.ProjectVariables[pair.Key][ProjectVariables.CommitCount] =
					pair.Value.CommitsAheadOfParent.Count().ToString();
			}

			return commitHistoryAnalysis;


		}
	}
}