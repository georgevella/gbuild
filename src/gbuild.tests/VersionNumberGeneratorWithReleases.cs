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
using Moq;
using Xunit;

namespace gbuild.tests
{
	public class VersionNumberGeneratorWithReleases
	{
		private readonly Fixture _fixture = new Fixture();
		private readonly Mock<IContextData<CommitHistoryAnalysis>> _commitAnalysisMock = new Mock<IContextData<CommitHistoryAnalysis>>();
		private readonly Mock<IContextData<WorkspaceDescription>> _workspaceContextDataMock = new Mock<IContextData<WorkspaceDescription>>();
		private readonly Mock<IBranchVersioningStrategyModel> _branchVersioningStrategyMock = new Mock<IBranchVersioningStrategyModel>();
		private readonly Mock<IWorkspaceConfiguration> _workspaceConfigurationMock = new Mock<IWorkspaceConfiguration>();

		private readonly Project _project1 = new Project("Project 1", new DirectoryInfo("src/project1/"));
		private readonly Project _project2 = new Project("Project 2", new DirectoryInfo("src/project2/"));

		private readonly SemanticVersion _project1ReleaseVersion = "1.2.0";
		private readonly SemanticVersion _project2ReleaseVersion = "2.4.0";

		public VersionNumberGeneratorWithReleases()
		{
			_workspaceConfigurationMock.SetupGet(x => x.StartingVersion).Returns("1.0.0");

			_branchVersioningStrategyMock.Setup(
				versionTag: "dev",
				versionMetadata: "metatag",
				parentBranch: "refs/heads/master",
				increment: VersionIncrementStrategy.Minor
			);



			// build workspace context data
			var latestRelease = new Release(
				_fixture.Create<DateTime>(),
				new Dictionary<Project, SemanticVersion>()
				{
					{_project1, _project1ReleaseVersion},
					{_project2, _project2ReleaseVersion}
				}
			);
			_workspaceContextDataMock.SetupGet(x => x.Data).Returns(
				new WorkspaceDescription(
					new DirectoryInfo("rootdir"),
					new DirectoryInfo("src"),
					new[]
					{
						_project1,
						_project2
					},
					new[]
					{
						latestRelease
					},
					_branchVersioningStrategyMock.Object,
					latestRelease.VersionNumbers
				)
			);
		}

		[Theory]
		// no changes
		[InlineData(0, 0, 0, 0)]
		// multi project change
		[InlineData(7, 10, 7, 3)]
		// single project change
		[InlineData(7, 10, 7, 0)]
		public void IndependentVersioning(
			int totalCommits,
			int totalChangedFiles,
			int project1Commits,
			int project2Commits
		)
		{
			// setup
			var changedProjects = new Dictionary<Project, int>();
			if (project1Commits > 0)
			{
				changedProjects[_project1] = project1Commits;
			}

			if (project2Commits > 0)
			{
				changedProjects[_project2] = project2Commits;
			}

			_commitAnalysisMock.Setup(_fixture, totalCommits, totalChangedFiles, changedProjects);

			var expectedProject1Version = SemanticVersion.CreateFrom(
				_project1ReleaseVersion.IncrementMinor(),
				prereleaseTag: $"dev-{project1Commits}",
				metadata: "metatag"
			);
			var expectedProject2Version = SemanticVersion.CreateFrom(
				_project2ReleaseVersion.IncrementMinor(),
				prereleaseTag: $"dev-{project2Commits}",
				metadata: "metatag"
			);

			var generator = new IndependentVersionNumberGenerator(
				_workspaceConfigurationMock.Object,
				_commitAnalysisMock.Object,
				_workspaceContextDataMock.Object
			);

			// act
			var project1Version = generator.GetVersion(_project1);
			var project2Version = generator.GetVersion(_project2);

			// verify
			project1Version.Should().Be(expectedProject1Version);
			project2Version.Should().Be(expectedProject2Version);

		}
	}
}