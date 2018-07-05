using System.IO;
using GBuild.Context;

namespace GBuild.Models
{
	public class Process : IContextEntity
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