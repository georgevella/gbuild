using System.Collections.Generic;
using System.Reflection;

namespace GBuild
{
	public class BuildCoreBootsrapperOptions
	{
		public List<Assembly> Assemblies { get; } = new List<Assembly>();
	}
}