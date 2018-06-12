namespace GBuild.Core.Generator
{
	public interface IVersionNumberGeneratorProvider
	{
		WorkspaceVersionNumbers GetVersion();
	}
}