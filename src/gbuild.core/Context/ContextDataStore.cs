using System;
using System.Collections.Generic;

namespace GBuild.Context
{
	public class ContextDataStore : IContextDataStore
	{
		private readonly Dictionary<Type, object> _store = new Dictionary<Type, object>();
		public T GetContextData<T>() where T : class
		{
			if (!_store.ContainsKey(typeof(T)))
				return null;

			return (T) _store[typeof(T)];
		}

		public void SetContextData<T>(
			T contextData
		)
		{
			_store[typeof(T)] = contextData;
		}
	}
}