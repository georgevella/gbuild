using GBuild.Configuration.Entities;
using GBuild.Models;
using GBuild.Variables;

namespace GBuild.Configuration.Models
{
	public interface IBranchVersioningStrategy
	{
		SemanticVersion Generate(
			SemanticVersion baseVersion,
			Project project
			);
	}

	class BranchVersioningStrategy : IBranchVersioningStrategy
	{
		private readonly IVariableRenderer _variableRenderer;

		public BranchVersioningStrategy(IVariableRenderer variableRenderer)
		{
			_variableRenderer = variableRenderer;
		}

		public string Tag { get; set; }

		public string Metadata { get; set; }

		public VersionIncrementStrategy Increment { get; set; }

		public SemanticVersion Generate(
			SemanticVersion baseVersion,
			Project project
		)
		{
			return SemanticVersion.Create(
				major: baseVersion.Major,
				minor: baseVersion.Minor,
				patch: baseVersion.Patch,
				prereleseTag: _variableRenderer.Render(Tag, project),
				metadata: _variableRenderer.Render(Metadata, project)
			);
		}
	}
}