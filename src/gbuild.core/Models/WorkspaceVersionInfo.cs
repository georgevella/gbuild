using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GBuild.Models
{
	public class WorkspaceVersionInfo : ReadOnlyDictionary<Project, SemanticVersion>
	{
		public WorkspaceVersionInfo(
			IDictionary<Project, SemanticVersion> dictionary
		)
			: base(dictionary)
		{
		}
	}
}