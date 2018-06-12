using System;
using System.IO;
using GBuild.Models;

namespace GBuild.Context.Providers
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