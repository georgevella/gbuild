using System.IO;

namespace GBuild.Context
{
	internal class ContextData<TContextData> : IContextData<TContextData> where TContextData : class
	{
		private readonly IContextDataStore _contextDataStore;

		public ContextData(
			IContextDataStore contextDataStore
		)
		{
			_contextDataStore = contextDataStore;
		}

		public TContextData Data => _contextDataStore.GetContextData<TContextData>();
	}

	public interface IContextDataStore
	{
		T GetContextData<T>() where T : class;

		void SetContextData<T>(T contextData);
	}
}