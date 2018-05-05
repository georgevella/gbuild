namespace GBuild.Core.Context
{
    public interface IContextData<out TContextData>
        where TContextData: class
    {
        TContextData Data { get; }
    }

    class ContextData<TContextData> : IContextData<TContextData> where TContextData : class
    {
        private readonly IContextDataProvider<TContextData> _contextDataProvider;
        private TContextData _data;

        public ContextData(IContextDataProvider<TContextData> contextDataProvider)
        {
            _contextDataProvider = contextDataProvider;
        }

        public TContextData Data => _data ?? (_data = _contextDataProvider.LoadContextData());
    }
}