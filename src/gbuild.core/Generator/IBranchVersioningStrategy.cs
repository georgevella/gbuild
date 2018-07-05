using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GBuild.CommitHistory;
using GBuild.Configuration;
using GBuild.Configuration.Entities;
using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	public interface IBranchVersioningStrategy
	{
		SemanticVersion Generate(
			IBranchVersioningSettings branchVersioningSettings,
			SemanticVersion baseVersion,
			Project project,
			IVariableStore variableStore
			);
	}

	public interface IBranchVersioningStrategyProvider
	{
		IBranchVersioningStrategy GetVersioningStrategy(
			string branchName
		);
		IBranchVersioningStrategy GetVersioningStrategy(
			IKnownBranch knownBranch
		);
	}

	class BranchVersioningStrategyProvider : IBranchVersioningStrategyProvider
	{
		private readonly IWorkspaceConfiguration _workspaceConfiguration;
		private readonly Dictionary<BranchType, IBranchVersioningStrategy> _branchVersioningStrategyMap;

		public BranchVersioningStrategyProvider(
			IEnumerable<IBranchVersioningStrategy> branchVersioningStrategies,
			IWorkspaceConfiguration workspaceConfiguration
			)
		{
			_workspaceConfiguration = workspaceConfiguration;
			_branchVersioningStrategyMap = branchVersioningStrategies.SelectMany(
				x => x.GetType().GetCustomAttributes<SupportedBranchTypeAttribute>().Select(b => new
				{
					BranchType = b.BranchType,
					Analyser = x
				})
			).ToDictionary(x => x.BranchType, x => x.Analyser);
		}
		public IBranchVersioningStrategy GetVersioningStrategy(
			string branchName
		)
		{
			var knownBranch = _workspaceConfiguration.KnownBranches.First(k => k.IsMatch(branchName));
			return GetVersioningStrategy(knownBranch);
		}

		public IBranchVersioningStrategy GetVersioningStrategy(
			IKnownBranch knownBranch
		)
		{
			return _branchVersioningStrategyMap[knownBranch.Type];
		}
	}
}