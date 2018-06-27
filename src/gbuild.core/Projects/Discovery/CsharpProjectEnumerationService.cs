using System.Xml;

namespace GBuild.Projects.Discovery
{
	public class CsharpProjectEnumerationService : BaseCsharpProjectEnumerationService
	{
		protected override bool IsSupportedFormat(XmlDocument doc)
		{
			var root = doc.DocumentElement;
			if (root == null)
				return false;

			return root.Name == "Project" && root.HasAttribute("Sdk");
		}
	}
}