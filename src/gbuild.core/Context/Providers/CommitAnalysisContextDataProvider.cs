using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Core.CommitAnalysis;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context.Data;
using GBuild.Core.Exceptions;
using GBuild.Core.Models;
using LibGit2Sharp;

namespace GBuild.Core.Context.Providers
{
	public class CommitAnalysisContextDataProvider : IContextDataProvider<CommitAnalysisResult>
	{
		private readonly ICommitHistoryAnalyser _commitHistoryAnalyser;


		public CommitAnalysisContextDataProvider(
			ICommitHistoryAnalyser commitHistoryAnalyser
		)
		{
			_commitHistoryAnalyser = commitHistoryAnalyser;
		}

		public CommitAnalysisResult LoadContextData()
		{
			return _commitHistoryAnalyser.Run();
		}
	}
}