using GBuild.Core.Configuration;

namespace GBuild.Core.Generator
{
	public interface IVersionNumberGenerator
	{
		SemanticVersion GetVersion(
			IBranchVersioningStrategyModel branchVersioningStrategyModel
		);
	}
}