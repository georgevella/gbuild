using System;
using System.IO;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GBuild.Core;
using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Generator;
using GBuild.Core.Models;
using LibGit2Sharp;
using Moq;
using Xunit;

namespace gbuild.tests
{
	public class VersionNumberGeneratorTests
	{
		[Fact]
		public void DevelopmentBranch_NoTags()
		{
			var fixture = new Fixture();

			var commitAnalysisMock = new Mock<IContextData<CommitAnalysisResult>>();
			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>();
			var workspaceConfigurationMock = new Mock<IWorkspaceConfiguration>();

			workspaceConfigurationMock.SetupGet(x => x.StartingVersion).Returns("1.0.0");

			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);

			var changedProjects = new[]
			{
				new Project("Test roject", new DirectoryInfo("testpath")),
			};

			commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitAnalysisResult(
						changedProjects,
						5,
						false, false
					)
				);

			var generator = new DevelopmentBranchVersionNumberGenerator(
				workspaceConfigurationMock.Object, 
				commitAnalysisMock.Object
				);

			var version = generator.GetVersion(branchVersioningStrategyMock.Object);

			var expectedVersion = SemanticVersion.CreateFrom(
				workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: "dev-5",
				metadata: "metatag"
			);

			version.Should().ContainKeys(changedProjects);
			version.Should().BeEquivalentTo(changedProjects.ToDictionary( x=>x, x=>expectedVersion) );
		}

		[Fact]
		public void DevelopmentBranch_WithTags()
		{
			var fixture = new Fixture();

			var commitAnalysisMock = new Mock<IContextData<CommitAnalysisResult>>();
			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>();
			var work = new Mock<IWorkspaceConfiguration>();

			work.SetupGet(x => x.StartingVersion).Returns("1.0.0");

			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);

			var changedProjects = new[]
			{
				new Project("Test roject", new DirectoryInfo("testpath")),
			};

			commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitAnalysisResult(
						changedProjects,
						5, 
						false, false
					)
				);

			var generator = new DevelopmentBranchVersionNumberGenerator(
				work.Object, 
				commitAnalysisMock.Object
				);

			var version = generator.GetVersion(branchVersioningStrategyMock.Object);

			var expectedVersion = SemanticVersion.CreateFrom(
				work.Object.StartingVersion,
				prereleaseTag: "dev-5",
				metadata: "metatag"
			);

			version.Should().ContainKeys(changedProjects);
			version.Should().BeEquivalentTo(changedProjects.ToDictionary(x => x, x => expectedVersion));

		}
	}
}