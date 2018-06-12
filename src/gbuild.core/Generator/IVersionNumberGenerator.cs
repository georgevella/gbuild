using GBuild.Models;

namespace GBuild.Generator
{
	public interface IVersionNumberGenerator
	{
		SemanticVersion GetVersion(Project project);
	}
}