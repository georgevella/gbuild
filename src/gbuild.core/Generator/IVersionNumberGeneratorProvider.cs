namespace GBuild.Core.Generator
{
	public interface IVersionNumberGeneratorProvider
	{
		SemanticVersion GetVersion();
	}
}