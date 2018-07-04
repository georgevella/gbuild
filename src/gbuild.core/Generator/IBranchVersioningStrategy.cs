using GBuild.Configuration.Entities;
using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	public interface IBranchVersioningStrategy
	{
		SemanticVersion Generate(
			IBranchVersioningSettings branchVersioningSettings,
			SemanticVersion baseVersion,
			Project project,
			IVariableStore variableStore
			);
	}
}