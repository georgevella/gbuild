namespace GBuild.Models
{
	public class ChangedFile
	{
		public ChangedFile(string path)
		{
			Path = path;
		}

		public string Path { get; }
	}
}