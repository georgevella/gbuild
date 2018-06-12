using System;
using System.IO;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;

namespace GBuild.Core.Context.Providers
{
	public class ProcessContextDataProvider : IContextDataProvider<Process>
	{
		public Process LoadContextData()
		{
			var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

			return new Process(currentDirectory);
		}
	}
}