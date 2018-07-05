using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GBuild.Assemblies
{
	public interface ITypeLookupService
	{
		IEnumerable<Type> GetAllTypesImplementing<TInterface>();
	}

	public class TypeLookupService : ITypeLookupService
	{
		private readonly List<Assembly> _assemblies;

		public TypeLookupService(IEnumerable<Assembly> assemblies)
		{
			_assemblies = assemblies.ToList();
		}

		public IEnumerable<Type> GetAllTypesImplementing<TInterface>()
		{
			var result = new List<Type>();
			_assemblies.ForEach( assembly =>
			{

				var typesImplementingIntf = assembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(TInterface)));
				result.AddRange(typesImplementingIntf);
			});

			return result;
		}
	}
}