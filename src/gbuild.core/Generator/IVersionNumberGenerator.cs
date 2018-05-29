using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;

namespace GBuild.Core.Generator
{
	public interface IVersionNumberGenerator
	{
		SemanticVersion GetVersion(
			IBranchVersioningStrategyModel branchVersioningStrategyModel
		);
	}
}