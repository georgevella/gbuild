using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.CommitHistory
{
	class BranchHistoryAnalyserProvider : IBranchHistoryAnalyserProvider
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly Dictionary<BranchType, IBranchHistoryAnalyser> _branchHistoryAnalyserMap;

		public BranchHistoryAnalyserProvider(
			IEnumerable<IBranchHistoryAnalyser> branchHistoryAnalysers,
			IWorkspaceConfiguration workspaceConfiguration
		)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_branchHistoryAnalyserMap = branchHistoryAnalysers.SelectMany(
				x => x.GetType().GetCustomAttributes<SupportedBranchTypeAttribute>().Select(b => new
				{
					BranchType = b.BranchType,
					Analyser = x
				})
			).ToDictionary(x => x.BranchType, x => x.Analyser);
		}
		public IBranchHistoryAnalyser GetBranchHistoryAnalyser(
			string branchName
		)
		{
			var knownBranch = _workspaceConfiguration.KnownBranches.First(k => k.IsMatch(branchName));
			return GetBranchHistoryAnalyser(knownBranch);
		}

		public IBranchHistoryAnalyser GetBranchHistoryAnalyser(
			IKnownBranch knownBranch
		)
		{
			return _branchHistoryAnalyserMap[knownBranch.Type];
		}
	}
}