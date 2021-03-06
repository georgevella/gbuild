﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Models;
using Moq;
using Xunit;
using Branch = LibGit2Sharp.Branch;

namespace gbuild.tests
{
	public class CommitAnalysisTests
	{
//		[Fact]
//		public void FactMethodName()
//		{
//			// setup
//			// current branch - develop, with it's own branch strategy
//			// repo has a tag on master, master branch and develop branch
//
//			var fixture = new Fixture();
//			var repoMock = new Mock<IGitRepository>(MockBehavior.Strict);
//			var workspaceMock = new Mock<IContextData<Workspace>>(MockBehavior.Strict);
//			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategy>(MockBehavior.Strict);
//			var workspaceConfigurationMock = new Mock<IWorkspaceConfiguration>(MockBehavior.Strict);
//
//			workspaceConfigurationMock.SetupGet(x => x.BranchVersioningStrategies).Returns(
//				() => new List<IBranchVersioningStrategy>()
//				{
//					branchVersioningStrategyMock.Object
//				});
//
//			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
//			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
//			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
//			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);
//
//			workspaceMock.SetupGet(x => x.Data).Returns(
//				new Workspace(
//					new DirectoryInfo(Environment.CurrentDirectory),
//					new DirectoryInfo("./src"),
//					new Project[]
//					{
//						new Project("Module1", new DirectoryInfo("./src/Module1"))
//					},
//					Enumerable.Empty<Release>(),
//					branchVersioningStrategyMock.Object
//					)
//
//			);
//
//			var sut = new GitCommitHistoryAnalyser(
//				repoMock.Object,
//				workspaceMock.Object
//			);
//
//			// test
//			var commitAnalysis = sut.AnalyseCommitLog();
//		}
	}
}