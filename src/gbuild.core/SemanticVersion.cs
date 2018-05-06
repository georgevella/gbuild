using System;
using System.Text;

namespace GBuild.Core
{
	public class SemanticVersion
	{
		internal SemanticVersion(
			int major,
			int minor,
			int patch,
			string prereleaseTag,
			string metadata
		)
		{
			Major = major;
			Minor = minor;
			Patch = patch;
			PrereleaseTag = prereleaseTag;
			Metadata = metadata;
		}

		public int Major { get; }

		public int Minor { get; }

		public int Patch { get; }

		public string PrereleaseTag { get; }

		public string Metadata { get; }

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append($"{Major}.{Minor}.{Patch}");

			if (!string.IsNullOrWhiteSpace(PrereleaseTag))
			{
				sb.Append($"-{PrereleaseTag}");
			}

			if (!string.IsNullOrWhiteSpace(Metadata))
			{
				sb.Append($"+{Metadata}");
			}

			return sb.ToString();
		}

		public static SemanticVersion Parse(
			string rawSemanticVersion
		)
		{
			throw new NotImplementedException();
		}

		public static SemanticVersion Create(
			int major = 0,
			int minor = 0,
			int patch = 0,
			string prereleseTag = null,
			string metadata = null
		)
		{
			// TODO: validate preprelease tag and metadata values

			return new SemanticVersion(major, minor, patch, prereleseTag, metadata);
		}
	}
}