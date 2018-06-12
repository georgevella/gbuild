using GBuild.Core.Configuration;
using GBuild.Core.Configuration.Models;
using GBuild.Core.Models;

namespace GBuild.Core.Generator
{
	public interface IVersionNumberGenerator
	{
		SemanticVersion GetVersion(Project project);
	}
}