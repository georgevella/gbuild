namespace GBuild.Console.Verbs
{
	public abstract class BaseVerb<T> : IVerb<T>
	{
		public abstract void Run(
			T options
		);
	}
}