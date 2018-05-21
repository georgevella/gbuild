using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using GBuild.Core;
using GBuild.Core.Configuration;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using GBuild.Core.Generator;
using GBuild.Core.Models;
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

			var commitAnalysisMock = new Mock<IContextData<CommitAnalysis>>();
			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategy>();
			var configurationFileMock = new Mock<IConfigurationFile>();

			configurationFileMock.SetupGet(x => x.StartingVersion).Returns("1.0.0");

			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);


			commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitAnalysis(
						Enumerable.Empty<Module>(),
						fixture.CreateMany<Commit>(5), 
						false, false
					)
				);

			var generator = new DevelopmentBranchVersionNumberGenerator(
				configurationFileMock.Object, 
				commitAnalysisMock.Object
				);

			var version = generator.GetVersion(branchVersioningStrategyMock.Object);

			var expectedVersion = SemanticVersion.CreateFrom(
				configurationFileMock.Object.StartingVersion,
				prereleaseTag: "dev-5",
				metadata: "metatag"
			);

			version.Should().Be(expectedVersion);
		}

		[Fact]
		public void DevelopmentBranch_WithTags()
		{
			var fixture = new Fixture();

			var commitAnalysisMock = new Mock<IContextData<CommitAnalysis>>();
			var branchVersioningStrategyMock = new Mock<IBranchVersioningStrategy>();
			var configurationFileMock = new Mock<IConfigurationFile>();

			configurationFileMock.SetupGet(x => x.StartingVersion).Returns("1.0.0");

			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns("dev");
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns("refs/heads/master");
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns("metatag");
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(VersionIncrementStrategy.Minor);


			commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitAnalysis(
						Enumerable.Empty<Module>(),
						fixture.CreateMany<Commit>(5), 
						false, false
					)
				);

			var generator = new DevelopmentBranchVersionNumberGenerator(
				configurationFileMock.Object, 
				commitAnalysisMock.Object
				);

			var version = generator.GetVersion(branchVersioningStrategyMock.Object);

			var expectedVersion = SemanticVersion.CreateFrom(
				configurationFileMock.Object.StartingVersion,
				prereleaseTag: "dev-5",
				metadata: "metatag"
			);

			version.Should().Be(expectedVersion);

		}
	}
}