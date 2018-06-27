using System.IO;

namespace GBuild.Models
{
	public abstract class BaseCsharpProject : Project
	{
		public FileInfo File { get; }

		public BaseCsharpProject(
			string name,
			FileInfo file
		)
			: base(name, file.Directory)
		{
			File = file;
		}
	}
}