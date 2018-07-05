using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GBuild.Assemblies;
using GBuild.Context.Attributes;

namespace GBuild.Context
{
	public interface IContextDataLoader
	{
		void PrepareContextData();
	}

	class ContextDataLoader : IContextDataLoader
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IContextDataStore _contextDataStore;
		private readonly ITypeLookupService _typeLookupService;

		public ContextDataLoader(
			IServiceProvider serviceProvider,
			IContextDataStore contextDataStore,
			ITypeLookupService typeLookupService
			)
		{
			_serviceProvider = serviceProvider;
			_contextDataStore = contextDataStore;
			_typeLookupService = typeLookupService;
		}

		public void PrepareContextData()
		{
			var queue = new Queue<Type>();

			var contextEntityTypes = _typeLookupService.GetAllTypesImplementing<IContextEntity>().ToList();
			var contextDataProviderMap = contextEntityTypes
				.Select(contextEntity =>
							new
							{
								ContextEntityType = contextEntity,
								ContextDataProviderType = typeof(IContextDataProvider<>).MakeGenericType(contextEntity)
							})
				.ToDictionary(x=>x.ContextEntityType, x=>x.ContextDataProviderType);

			contextEntityTypes.ForEach( t => BuildEntityTypeResolutionHierarchy(queue, t) );

			var fetchContextDataGenericMethod =
				this.GetType().GetRuntimeMethods().FirstOrDefault(m => m.Name == nameof(FetchContextData));

			while (queue.Count > 0)
			{
				var contextEntityType = queue.Dequeue();
				var contextDataProvider = _serviceProvider.GetService(contextDataProviderMap[contextEntityType]);

				var actualFetchContextData = fetchContextDataGenericMethod.MakeGenericMethod(contextEntityType);

				actualFetchContextData.Invoke(this, new[] { contextDataProvider });
			}			
		}

		private void BuildEntityTypeResolutionHierarchy(
			Queue<Type> queue,
			Type type
		)
		{
			var contextEntityTypes = type.GetCustomAttributes<DependsOnContextDataAttribute>().Select(t => t.ContextEntity).ToList();
			if (contextEntityTypes.Any())
			{
				contextEntityTypes.ForEach( t => BuildEntityTypeResolutionHierarchy(queue, t));
			}

			if (queue.Contains(type))
				return;

			queue.Enqueue(type);

		}

		private void FetchContextData<T>(IContextDataProvider<T> contextDataProvider) where T : class, IContextEntity
		{
			var contextData = contextDataProvider.LoadContextData();
			_contextDataStore.SetContextData(contextData);
		}
	}
}