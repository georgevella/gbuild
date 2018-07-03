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
			Project project
			);
	}

	class BranchVersioningStrategy : IBranchVersioningStrategy
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
			Project project
		)
		{
			return SemanticVersion.Create(
				major: baseVersion.Major,
				minor: baseVersion.Minor,
				patch: baseVersion.Patch,
				prereleseTag: _variableRenderer.Render(branchVersioningSettings.Tag, project),
				metadata: _variableRenderer.Render(branchVersioningSettings.Metadata, project)
			);
		}
	}
}