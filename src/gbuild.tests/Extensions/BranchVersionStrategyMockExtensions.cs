using GBuild.Configuration.Models;
using Moq;

namespace gbuild.tests.Extensions
{
	public static class BranchVersionStrategyMockExtensions
	{
		public static void Setup(
			this Mock<IBranchVersioningStrategyModel> branchVersioningStrategyMock,
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
	}
}