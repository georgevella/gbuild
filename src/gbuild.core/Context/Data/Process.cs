using System.IO;

namespace GBuild.Core.Context.Data
{
	public class Process
	{
		public Process(
			DirectoryInfo currentDirectory
		)
		{
			CurrentDirectory = currentDirectory;
		}

		public DirectoryInfo CurrentDirectory { get; }
	}
}