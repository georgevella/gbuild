using GBuild.Models;

namespace GBuild.Generator
{
	public interface IVersionNumberGeneratorProvider
	{
		WorkspaceVersionInfo GetVersion();
	}
}