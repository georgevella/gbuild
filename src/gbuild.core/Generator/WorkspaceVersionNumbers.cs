using System.Collections.Generic;
using System.Collections.ObjectModel;
using GBuild.Models;

namespace GBuild.Generator
{
	public class WorkspaceVersionNumbers : ReadOnlyDictionary<Project, SemanticVersion>
	{
		public WorkspaceVersionNumbers(
			IDictionary<Project, SemanticVersion> dictionary
		)
			: base(dictionary)
		{
		}
	}
}