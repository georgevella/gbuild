using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using gbuild.tests.Extensions;
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
		private Fixture _fixture = new Fixture();
		private Mock<IContextData<CommitHistoryAnalysis>> _commitAnalysisMock = new Mock<IContextData<CommitHistoryAnalysis>>();
		private Mock<IContextData<Workspace>> _workspaceContextDataMock = new Mock<IContextData<Workspace>>();
		private Mock<IBranchVersioningStrategyModel> _branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>();
		private Mock<IWorkspaceConfiguration> _workspaceConfigurationMock = new Mock<IWorkspaceConfiguration>();
		private Project _project1 = new Project("Project 1", new DirectoryInfo("src/project1/"));
		private Project _project2 = new Project("Project 2", new DirectoryInfo("src/project2/"));

		public VersionNumberGeneratorTests()
		{
			_workspaceConfigurationMock.SetupGet(x => x.StartingVersion).Returns("1.0.0");

			_branchVersioningStrategyMock.Setup(
				versionTag: "dev",
				versionMetadata: "metatag",
				parentBranch: "refs/heads/master",
				increment: VersionIncrementStrategy.Minor
			);



			// build workspace context data
			_workspaceContextDataMock.SetupGet(x => x.Data).Returns(
				new Workspace(
					new DirectoryInfo("rootdir"),
					new DirectoryInfo("src"),
					new[]
					{
						_project1,
						_project2
					}
				)
			);
		}

		[Fact]
		public void Independent_NoTags_NoChanges()
		{
			// setup
			const int project1Commits = 0;
			const int project2Commits = 0;

			_commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitHistoryAnalysis(
						new Dictionary<Project, List<Commit>>(), 
						_fixture.CreateMany<Commit>(7),
						_fixture.CreateMany<ChangedFile>(5),
						false,
						false,
						_branchVersioningStrategyMock.Object
					)
				);

			var generator = new IndependentVersionNumberGenerator(
				_workspaceConfigurationMock.Object,
				_commitAnalysisMock.Object,
				_workspaceContextDataMock.Object
			);

			// act
			var project1Version = generator.GetVersion(_project1);
			var project2Version = generator.GetVersion(_project2);

			var expectedProject1Version = SemanticVersion.CreateFrom(
				_workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: $"dev-{project1Commits}",
				metadata: "metatag"
			);
			var expectedProject2Version = SemanticVersion.CreateFrom(
				_workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: $"dev-{project2Commits}",
				metadata: "metatag"
			);

			// verify
			project1Version.Should().Be(expectedProject1Version);
			project2Version.Should().Be(expectedProject2Version);
		}

		[Fact]
		public void Independent_NoTags_MultiProject()
		{
			// setup
			const int project1Commits = 7;
			const int project2Commits = 3;

			var changedProjects = new Dictionary<Project, List<Commit>>()
			{
				{
					_project1,
					new List<Commit>(_fixture.CreateMany<Commit>(project1Commits))
				},
				{
					_project2,
					new List<Commit>(_fixture.CreateMany<Commit>(project2Commits))
				}
			};

			_commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitHistoryAnalysis(
						changedProjects,
						_fixture.CreateMany<Commit>(7),
						_fixture.CreateMany<ChangedFile>(5),
						false,
						false,
						_branchVersioningStrategyMock.Object
					)
				);

			var generator = new IndependentVersionNumberGenerator(
				_workspaceConfigurationMock.Object,
				_commitAnalysisMock.Object,
				_workspaceContextDataMock.Object
			);

			// act
			var project1Version = generator.GetVersion(_project1);
			var project2Version = generator.GetVersion(_project2);

			var expectedProject1Version = SemanticVersion.CreateFrom(
				_workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: $"dev-{project1Commits}",
				metadata: "metatag"
			);
			var expectedProject2Version = SemanticVersion.CreateFrom(
				_workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: $"dev-{project2Commits}",
				metadata: "metatag"
			);

			// verify
			project1Version.Should().Be(expectedProject1Version);
			project2Version.Should().Be(expectedProject2Version);
		}

		[Fact]
		public void Independent_NoTags_SingleProjectChange()
		{
			// setup
			const int project2Commits = 3;

			var changedProjects = new Dictionary<Project, List<Commit>>()
			{
				{
					_project2, 
					new List<Commit>(_fixture.CreateMany<Commit>(project2Commits))
				}
			};

			_commitAnalysisMock.SetupGet(x => x.Data)
				.Returns(
					new CommitHistoryAnalysis(
						changedProjects,
						_fixture.CreateMany<Commit>(5),
						_fixture.CreateMany<ChangedFile>(5),
						false, 
						false,
						_branchVersioningStrategyMock.Object
					)
				);

			var generator = new IndependentVersionNumberGenerator(
				_workspaceConfigurationMock.Object, 
				_commitAnalysisMock.Object,
				_workspaceContextDataMock.Object
				);

			// act
			var project1Version = generator.GetVersion(_project1);
			var project2Version = generator.GetVersion(_project2);

			var expectedProject1Version = SemanticVersion.CreateFrom(
				_workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: $"dev-0",
				metadata: "metatag"
			);
			var expectedProject2Version = SemanticVersion.CreateFrom(
				_workspaceConfigurationMock.Object.StartingVersion,
				prereleaseTag: $"dev-{project2Commits}",
				metadata: "metatag"
			);

			// verify
			project1Version.Should().Be(expectedProject1Version);
			project2Version.Should().Be(expectedProject2Version);
		}

		[Fact]
		public void DevelopmentBranch_WithTags()
		{
//			var fixture = new Fixture();
//
//			var commitAnalysisMock = new Mock<IContextData<CommitHistoryAnalysis>>();
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
//					new CommitHistoryAnalysis(
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