using System;
using System.IO;
using GBuild.Core.Context.Data;

namespace GBuild.Core.Context.Providers
{
	public class ProcessInformationContextDataProvider : IContextDataProvider<ProcessInformation>
	{
		public ProcessInformation LoadContextData()
		{
			var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);

			return new ProcessInformation(currentDirectory);
		}
	}
}