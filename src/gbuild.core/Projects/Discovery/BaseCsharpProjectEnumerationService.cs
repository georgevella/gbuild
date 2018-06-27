using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GBuild.Models;

namespace GBuild.Projects.Discovery
{
	public abstract class BaseCsharpProjectEnumerationService : IProjectEnumerationService
	{
		/// <inheritdoc />
		public IEnumerable<Project> GetProjects(DirectoryInfo sourceCodeRootDirectory)
		{
			var projectFiles = sourceCodeRootDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories).ToList();

			return projectFiles
				.Select(fileInfo => new
				{
					File = fileInfo,
					Document = LoadProject(fileInfo)
				})
				.Where(x => IsSupportedFormat(x.Document))
				.Select(x => new CsharpProject(Path.GetFileNameWithoutExtension(x.File.Name), x.File))
				.ToList();
		}

		protected abstract bool IsSupportedFormat(XmlDocument doc);

		private XmlDocument LoadProject(FileInfo file)
		{
			using (var reader = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				var doc = new XmlDocument();
				doc.Load(reader);

				return doc;
			}
		}
	}
}