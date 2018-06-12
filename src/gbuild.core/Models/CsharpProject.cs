using System.IO;

namespace GBuild.Models
{
	public class CsharpProject : Project
	{
		public FileInfo File { get; }

		public ModuleType Type { get; }

		public CsharpProject(
			string name,
			FileInfo file,
			ModuleType type
		)
			: base(name, file.Directory)
		{
			File = file;
			Type = type;
		}
	}
}