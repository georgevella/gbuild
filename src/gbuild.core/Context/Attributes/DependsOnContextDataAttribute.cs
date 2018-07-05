using System;

namespace GBuild.Context.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class DependsOnContextDataAttribute : Attribute
	{
		public Type ContextEntity { get; }

		public DependsOnContextDataAttribute(Type contextEntity)
		{
			ContextEntity = contextEntity;
		}
	}
}