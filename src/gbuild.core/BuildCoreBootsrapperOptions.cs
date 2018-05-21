using System;
using System.Collections.Generic;
using System.Reflection;

namespace GBuild.Core
{
	public class BuildCoreBootsrapperOptions
	{
		public List<Assembly> Assemblies { get; } = new List<Assembly>();

		public Type RepositoryType { get; set; }
	}
}