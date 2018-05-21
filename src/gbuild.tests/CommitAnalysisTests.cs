using System;
using System.IO;
using AutoFixture;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Context.Providers;
using GBuild.Core.Models;
using GBuild.Core.Vcs;
using Moq;
using Xunit;

namespace gbuild.tests
{
	public class CommitAnalysisTests
	{
		[Fact]
		public void FactMethodName()
		{
			// setup
			// current branch - develop, with it's own branch strategy
			// repo has a tag on master, master branch and develop branch

			var fixture = new Fixture();
			var repoMock = new Mock<ISourceCodeRepository>(MockBehavior.Strict);
			var currentBranchInformationMock = new Mock<IContextData<BranchInformation>>(MockBehavior.Strict);
			var projectInformationMock = new Mock<IContextData<ProjectInformation>>(MockBehavior.Strict);
			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>(MockBehavior.Strict);

			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);

			var developBranch = new Branch("refs/heads/develop", fixture.Create<Commit>());
			var masterBranch = new Branch("refs/heads/master", fixture.Create<Commit>());
			var tagOnMaster = new Tag("refs/tags/1.2.0", masterBranch.LatestCommit);

			repoMock.SetupGet(x => x.Branches).Returns(
				new []
				{
					developBranch,
					masterBranch, 
				}
			);

			repoMock.SetupGet(x => x.Tags).Returns(new[]
			{
				tagOnMaster
			});

			repoMock.SetupGet(x => x.CurrentBranch).Returns(developBranch);

			repoMock.Setup(
					x => x.GetCommitsBetween(
						It.Is<Branch>(b => b.Equals(masterBranch)),
						It.Is<Branch>(b => b.Equals(developBranch))
					)
				)
				.Returns(fixture.CreateMany<Commit>(5));

			currentBranchInformationMock.SetupGet(x => x.Data)
				.Returns(new BranchInformation(developBranch, branchVersioningStrategyMock.Object));

			projectInformationMock.SetupGet(x => x.Data).Returns(
				new ProjectInformation(
					new DirectoryInfo(Environment.CurrentDirectory),
					new DirectoryInfo("./src"),
					new Module[]
					{
						new Module("Module1"),
					})
			);

			var sut = new CommitAnalysisContextDataProvider(
				repoMock.Object,
				currentBranchInformationMock.Object,
				projectInformationMock.Object
			);

			// test
			var commitAnalysis = sut.LoadContextData();			
		}
	}
}