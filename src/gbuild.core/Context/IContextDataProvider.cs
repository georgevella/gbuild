namespace GBuild.Context
{
	public interface IContextDataProvider<out TContextData>
		where TContextData : class
	{
		TContextData LoadContextData();
	}
}