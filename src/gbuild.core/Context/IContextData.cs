namespace GBuild.Context
{
	public interface IContextData<out TContextData>
		where TContextData : class
	{
		TContextData Data { get; }
	}
}