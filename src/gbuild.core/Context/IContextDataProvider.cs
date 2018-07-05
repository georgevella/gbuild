namespace GBuild.Context
{
	public interface IContextDataProvider
	{

	}

	public interface IContextDataProvider<out TContextData> : IContextDataProvider
		where TContextData : class, IContextEntity
	{
		TContextData LoadContextData();
	}
}