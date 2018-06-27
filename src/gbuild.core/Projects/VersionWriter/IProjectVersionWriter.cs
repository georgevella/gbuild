using System;
using GBuild.Models;

namespace GBuild.Projects.VersionWriter
{
	public interface IProjectVersionWriter
	{
		void UpdateVersionInformation(Project project, SemanticVersion version);
	}
}