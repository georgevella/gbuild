using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GBuild.Models;

namespace GBuild.Projects.VersionWriter
{
	/// <summary>
	///		Writes a Directory.Build.targets file into the project to set the project version.
	/// </summary>
	public class CsharpProjectVersionWriter : IProjectVersionWriter
	{
		/// <inheritdoc />
		public void UpdateVersionInformation(Project project, SemanticVersion version)
		{
			var projectPath = project.Path;
			var directoryBuildProps = Path.Combine(projectPath.FullName, "Directory.Build.props");

			XmlElement projectElement = null;
			XmlElement versionPropertyGroupElement = null;
			XmlElement versionPrefixElement = null;
			XmlElement versionSuffixElement = null;

			var doc = new XmlDocument();
			var fi = new FileInfo(directoryBuildProps);

			if (fi.Exists)
			{
				using (var reader = fi.OpenRead())
				{
					doc.Load(reader);
				}

				versionPrefixElement = doc.SelectSingleNode("/Project/PropertyGroup/VersionPrefix") as XmlElement;
				versionSuffixElement = doc.SelectSingleNode("/Project/PropertyGroup/VersionSuffix") as XmlElement;

				if (versionPrefixElement != null)
				{
					versionPropertyGroupElement = (XmlElement) versionPrefixElement.ParentNode;
					projectElement = (XmlElement) versionPropertyGroupElement?.ParentNode;
				}

				if (versionSuffixElement != null && versionPropertyGroupElement == null)
				{
					versionPropertyGroupElement = (XmlElement) versionSuffixElement.ParentNode;
					projectElement = (XmlElement)versionPropertyGroupElement?.ParentNode;
				}
			}

			if (projectElement == null)
			{
				projectElement = doc.CreateElement("Project");
				doc.AppendChild(projectElement);
			}

			if (versionPropertyGroupElement == null)
			{
				versionPropertyGroupElement = (XmlElement)projectElement
					.AppendChild(doc.CreateElement("PropertyGroup"));
			}
			
			if (versionPrefixElement == null)
			{

				versionPrefixElement = (XmlElement)versionPropertyGroupElement
					.AppendChild(doc.CreateElement("VersionPrefix"));
			}

			if (versionSuffixElement == null)
			{
				versionSuffixElement = (XmlElement)versionPropertyGroupElement
					.AppendChild(doc.CreateElement("VersionSuffix"));
			}
			
			versionPrefixElement.InnerText = $"{version.Major}.{version.Minor}.{version.Patch}";
			var versionSuffix = new StringBuilder();
			if (version.PrereleaseTag.Any())
			{
				versionSuffix.Append(version.PrereleaseTag);
			}

			if (version.Metadata.Any())
			{
				versionSuffix.Append("+");
				versionSuffix.Append(version.Metadata);
			}

			versionSuffixElement.InnerText = $"{versionSuffix}";

			doc.Save(directoryBuildProps);
		}
	}
}