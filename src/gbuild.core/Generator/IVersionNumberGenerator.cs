using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;

namespace GBuild.Core.Generator
{
	public interface IVersionNumberGenerator
	{
		WorkspaceVersionNumbers GetVersion(
			IBranchVersioningStrategyModel branchVersioningStrategyModel
		);
	}
}