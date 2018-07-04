using GBuild.Configuration.Models;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Generator
{
	public class BranchVersioningStrategy : IBranchVersioningStrategy
	{
		private readonly IVariableRenderer _variableRenderer;

		public BranchVersioningStrategy(
			IVariableRenderer variableRenderer
		)
		{
			_variableRenderer = variableRenderer;
		}

		public SemanticVersion Generate(
			IBranchVersioningSettings branchVersioningSettings,
			SemanticVersion baseVersion,
			Project project,
			IVariableStore variableStore
		)
		{
			return SemanticVersion.Create(
				major: baseVersion.Major,
				minor: baseVersion.Minor,
				patch: baseVersion.Patch,
				prereleseTag: _variableRenderer.Render(branchVersioningSettings.Tag, project, variableStore),
				metadata: _variableRenderer.Render(branchVersioningSettings.Metadata, project, variableStore)
			);
		}
	}
}