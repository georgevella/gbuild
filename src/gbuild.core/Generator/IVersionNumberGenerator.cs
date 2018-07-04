using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	public interface IVersionNumberGenerator
	{
		SemanticVersion GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis, 
			IBranchVersioningStrategy branchVersioningStrategy,
			IBranchVersioningSettings branchVersioningSettings,
			Project project,
			IVariableStore variableStore
			);
	}
}