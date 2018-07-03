using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using GBuild.Configuration.Entities;
using GBuild.Configuration.Models;
using GBuild.Context;
using GBuild.Generator;
using GBuild.Models;
using Moq;

namespace gbuild.tests.Extensions
{
	public static class MockExtensions
	{
		public static void Setup(
			this Mock<IBranchVersioningStrategy> branchVersioningStrategyMock,
			string versionTag = null,
			string versionMetadata = null,
			string parentBranch = null,		
			VersionIncrementStrategy increment = VersionIncrementStrategy.Minor
		)
		{
			branchVersioningStrategyMock.SetupGet(x => x.Tag).Returns(versionTag);
			branchVersioningStrategyMock.SetupGet(x => x.ParentBranch).Returns(parentBranch);
			branchVersioningStrategyMock.SetupGet(x => x.Metadata).Returns(versionMetadata);
			branchVersioningStrategyMock.SetupGet(x => x.Increment).Returns(increment);
		}

		public static void Setup(
			this Mock<IContextData<CommitHistoryAnalysis>> mock,
			IFixture fixture,
			int commits,
			int changedFiles,
			IDictionary<Project, int> projectChanges = null
		)
		{
			var changedProjects =
				projectChanges?.ToDictionary(x => x.Key, x => new ChangedProject(fixture.CreateMany<Commit>(x.Value), false, false)) ??
				new Dictionary<Project, ChangedProject>();


			mock.SetupGet(x => x.Data)
				.Returns(
					new CommitHistoryAnalysis(
						changedProjects,
						fixture.CreateMany<Commit>(commits),
						fixture.CreateMany<ChangedFile>(changedFiles),
						false,
						false
					)
				);
		}

		public static void Setup(
			this Mock<IContextData<Releases>> mock,
			IEnumerable<Release> pastReleases = null,
			IEnumerable<Release> activeReleases = null
		)
		{
			mock.SetupGet(x => x.Data).Returns(
				new Releases(
					pastReleases ?? Enumerable.Empty<Release>(),
					activeReleases ?? Enumerable.Empty<Release>()
				));
		}
	}
}