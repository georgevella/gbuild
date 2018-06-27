using System.IO;

namespace GBuild.Models
{
	public class CsharpProject : BaseCsharpProject
	{
		/// <inheritdoc />
		public CsharpProject(string name, FileInfo file) : base(name, file)
		{
		}
	}
}