using System.IO;

namespace GBuild.Models
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