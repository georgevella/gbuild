using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GBuild.Models
{
	public class WorkspaceVersionInfo : ReadOnlyDictionary<Project, SemanticVersion>
	{
		public static WorkspaceVersionInfo Empty() => new WorkspaceVersionInfo(new Dictionary<Project, SemanticVersion>());

		public WorkspaceVersionInfo(
			IDictionary<Project, SemanticVersion> dictionary
		)
			: base(dictionary)
		{
		}
	}
}