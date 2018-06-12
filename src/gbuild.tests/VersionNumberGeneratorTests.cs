using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GBuild;
using GBuild.Configuration;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using LibGit2Sharp;
using Moq;
using Xunit;
using Commit = GBuild.Models.Commit;

namespace gbuild.tests
{
	public class VersionNumberGeneratorTests
	{
		[Fact]
		public void Independent_NoTags_NoChanges()
		{

		}

		[Fact]
		public void Independent_NoTags_MultiProject()
		{

		}

		[Fact]
		public void Independent_NoTags_SingleProject()
		{
			var fixture = new Fixture();

			var commitAnalysisMock = new Mock<IContextData<CommitAnalysisResult>>();
			var workspaceContextDataMock = new Mock<IContextData<Workspace>>();
			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>();
			var workspaceConfigurationMock = new Mock<IWorkspaceConfiguration>();

			workspaceConfigurationMock.SetupGet(x => x.StartingVersion).Returns("1.0.0");
			
			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);

			var project1 = new Project("Project 1", new DirectoryInfo("src/project1/"));
			var project2 = new Project("Project 2", new DirectoryInfo("src/project2/"));

			// build workspace context data
			workspaceContextDataMock.SetupGet(x => x.Data).Returns(
				new Workspace(
					new DirectoryInfo("rootdir"),
					new DirectoryInfo("src"),
					new[]
					{
						project1,
						project2
					}
				)
			);

			var changedProjects = new Dictionary<Project, List<Commit>>()
			{
				{
					project2, 
					new List<Commit>(fixture.CreateMany<Commit>(3))
				}
			};

			commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitAnalysisResult(
						changedProjects,
						fixture.CreateMany<Commit>(5),
						fixture.CreateMany<ChangedFile>(5),
						false, 
						false,
						branchVersioningStrategyMock.Object
					)
				);

			var generator = new IndependentVersionNumberGenerator(
				workspaceConfigurationMock.Object, 
				commitAnalysisMock.Object,
				workspaceContextDataMock.Object
				);

			var version = generator.GetVersion(project2);

			var expectedVersion = SemanticVersion.CreateFrom(
				workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: "dev-5",
				metadata: "metatag"
			);

			version.Should().Be(expectedVersion);
		}

		[Fact]
		public void DevelopmentBranch_WithTags()
		{
//			var fixture = new Fixture();
//
//			var commitAnalysisMock = new Mock<IContextData<CommitAnalysisResult>>();
//			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>();
//			var work = new Mock<IWorkspaceConfiguration>();
//
//			work.SetupGet(x => x.StartingVersion).Returns("1.0.0");
//
//			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
//			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
//			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
//			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);
//
//			var project1 = new Project("Project 1", new DirectoryInfo("src/project1/"));
//			var project2 = new Project("Project 2", new DirectoryInfo("src/project2/"));
//
//			var changedProjects = new[]
//			{
//				new Project("Test roject", new DirectoryInfo("testpath")),
//			};
//
//			commitAnalysisMock.SetupGet(x => x.Data)
//				.Returns(
//					new CommitAnalysisResult(
//						changedProjects,
//						5, 
//						false, false
//					)
//				);
//
//			var generator = new IndependentVersionNumberGenerator(
//				work.Object, 
//				commitAnalysisMock.Object
//				);
//
//			var version = generator.GetVersion(project2);
//
//			var expectedVersion = SemanticVersion.CreateFrom(
//				work.Object.StartingVersion,
//				prereleaseTag: "dev-5",
//				metadata: "metatag"
//			);
//			version.Should().Be(expectedVersion);
		}
	}
}