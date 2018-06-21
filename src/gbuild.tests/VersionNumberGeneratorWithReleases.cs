﻿using System;
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
		private readonly Mock<IContextData<Workspace>> _workspaceContextDataMock = new Mock<IContextData<Workspace>>();
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
			_workspaceContextDataMock.SetupGet(x => x.Data).Returns(
				new Workspace(
					new DirectoryInfo("rootdir"),
					new DirectoryInfo("src"),
					new[]
					{
						_project1,
						_project2
					},
					new[]
					{
						new Release(
							_fixture.Create<DateTime>(),
							new Dictionary<Project, SemanticVersion>()
							{
								{ _project1, _project1ReleaseVersion },
								{ _project2, _project2ReleaseVersion }
							}
						),
					}
				)
			);
		}		

		[Fact]
		public void Independent_WithTags_SingleProjectChange_NoBreakingChanges()
		{
			const int PROJECT1_COMMITS = 0;
			const int PROJECT2_COMMITS = 3;			

			// expected
			var expectedProject1Version = SemanticVersion.CreateFrom(
				_project1ReleaseVersion.IncrementMinor(),
				prereleaseTag: $"dev-{PROJECT1_COMMITS}",
				metadata: "metatag"
			);
			var expectedProject2Version = SemanticVersion.CreateFrom(
				_project2ReleaseVersion.IncrementMinor(),
				prereleaseTag: $"dev-{PROJECT2_COMMITS}",
				metadata: "metatag"
			);

			// setup			

			var changedProjects = new Dictionary<Project, List<Commit>>()
			{
				{
					_project2,
					new List<Commit>(_fixture.CreateMany<Commit>(PROJECT2_COMMITS))
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

			// verify
			project1Version.Should().Be(expectedProject1Version);
			project2Version.Should().Be(expectedProject2Version);
		}
	}
}