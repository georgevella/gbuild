using GBuild.CommitHistory;
using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	[SupportedBranchType(BranchType.Release)]
	public class ReleaseBranchVersioningStrategy : IBranchVersioningStrategy
	{
		public SemanticVersion Generate(
			IBranchVersioningSettings branchVersioningSettings,
			SemanticVersion baseVersion,
			Project project,
			IVariableStore variableStore
		)
		{
			return baseVersion;
		}
	}
}