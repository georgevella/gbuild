namespace GBuild.Context
{
	/// <summary>
	///		Contract used by context data consumers.
	/// </summary>
	/// <typeparam name="TContextData"></typeparam>
	public interface IContextData<out TContextData>
		where TContextData : class
	{
		TContextData Data { get; }
	}
}