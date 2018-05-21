using System.IO;

namespace GBuild.Core.Models
{
	public class CsharpModule : Module
	{
		public FileInfo File { get; }

		public ModuleType Type { get; }

		public CsharpModule(
			string name,
			FileInfo file,
			ModuleType type
		)
			: base(name)
		{
			File = file;
			Type = type;
		}
	}
}