using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	public interface IVersionNumberGenerator
	{
		SemanticVersion GetVersion(
			CommitHistoryAnalysis commitHistoryAnalysis, 			
			Project project,
			IVariableStore variableStore
			);
	}
}