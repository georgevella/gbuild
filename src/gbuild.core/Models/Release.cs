using System;
using System.Collections.Generic;

namespace GBuild.Models
{
	public class Release
	{
		public DateTime When { get; }

		public WorkspaceVersionInfo VersionNumbers { get; }

		public Release(
			DateTime @when,
			WorkspaceVersionInfo versionNumbers
		)
		{
			When = when;
			VersionNumbers = versionNumbers;
		}

		public Release(
			DateTime @when,
			Dictionary<Project, SemanticVersion> versionNumbers
		)
		: this(when, new WorkspaceVersionInfo(versionNumbers))
		{
			
		}
	}
}