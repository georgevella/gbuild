namespace GBuild.Core.Context
{
    public interface IContextDataProvider<out TContextData>
        where TContextData : class
    {
        TContextData LoadContextData();
    }
}