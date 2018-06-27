using System.IO;

namespace GBuild.Models
{
	public class LegacyCsharpProject : BaseCsharpProject
	{
		/// <inheritdoc />
		public LegacyCsharpProject(string name, FileInfo file) : base(name, file)
		{
		}
	}
}