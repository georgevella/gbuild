namespace GBuild.Console
{
	public interface IVerb<in TVerbOptions>
	{
		void Run(
			TVerbOptions options
		);
	}
}