namespace GBuild.Core.Context
{
	public interface IContextData<out TContextData>
		where TContextData : class
	{
		TContextData Data { get; }
	}
}