using System;
using System.Collections.Generic;
using System.Text;

namespace GBuild
{
	public class SemanticVersion
	{
		private SemanticVersion()
		{
		}

		internal SemanticVersion(
			int major,
			int minor,
			int patch,
			string prereleaseTag = null,
			string metadata = null
		)
		{
			Major = major;
			Minor = minor;
			Patch = patch;
			PrereleaseTag = prereleaseTag ?? string.Empty;
			Metadata = metadata ?? string.Empty;
		}

		public int Major { get; private set; }

		public int Minor { get; private set; }

		public int Patch { get; private set; }

		public string PrereleaseTag { get; private set; }

		public string Metadata { get; private set; }

		protected bool Equals(
			SemanticVersion other
		)
		{
			return Major == other.Major && Minor == other.Minor && Patch == other.Patch &&
				   string.Equals(PrereleaseTag, other.PrereleaseTag) && string.Equals(Metadata, other.Metadata);
		}

		public override bool Equals(
			object obj
		)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((SemanticVersion) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Major;
				hashCode = (hashCode * 397) ^ Minor;
				hashCode = (hashCode * 397) ^ Patch;
				hashCode = (hashCode * 397) ^ (PrereleaseTag != null ? PrereleaseTag.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Metadata != null ? Metadata.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static implicit operator SemanticVersion(
			string v
		)
		{
			return Parse(v);
		}

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
			var buffers = new List<Tuple<ParsingMode, StringBuilder, Action<SemanticVersion, string>>>
			{
				new Tuple<ParsingMode, StringBuilder, Action<SemanticVersion, string>>(
					ParsingMode.Numeric,
					new StringBuilder(),
					(
						version,
						value
					) => version.Major = int.Parse(value)), // major
				new Tuple<ParsingMode, StringBuilder, Action<SemanticVersion, string>>(
					ParsingMode.Numeric,
					new StringBuilder(),
					(
						version,
						value
					) => version.Minor = int.Parse(value)), // minor
				new Tuple<ParsingMode, StringBuilder, Action<SemanticVersion, string>>(
					ParsingMode.Numeric,
					new StringBuilder(),
					(
						version,
						value
					) => version.Patch = int.Parse(value)), // patch
				new Tuple<ParsingMode, StringBuilder, Action<SemanticVersion, string>>(
					ParsingMode.Tag,
					new StringBuilder(),
					(
						version,
						value
					) => version.PrereleaseTag = value), // tag
				new Tuple<ParsingMode, StringBuilder, Action<SemanticVersion, string>>(
					ParsingMode.Metadata,
					new StringBuilder(),
					(
						version,
						value
					) => version.Metadata = value) // metadata
			};

			using (var bufferEnumerator = buffers.GetEnumerator())
			{
				bufferEnumerator.MoveNext();
				foreach (var ch in rawSemanticVersion)
				{
					if (bufferEnumerator.Current == null)
						throw new InvalidOperationException();

					switch (bufferEnumerator.Current.Item1)
					{
						case ParsingMode.Numeric:
							switch (ch)
							{
								case '.':
								case '-':
								case '+':
									bufferEnumerator.MoveNext();
									break;
								default:

									if (char.IsDigit(ch))
									{
										bufferEnumerator.Current.Item2.Append(ch);
									}

									break;
							}
							break;

						case ParsingMode.Tag:
							if (ch == '+')
							{
								bufferEnumerator.MoveNext();
							}
							else
							{
								bufferEnumerator.Current.Item2.Append(ch);
							}

							break;

						case ParsingMode.Metadata:
							bufferEnumerator.Current.Item2.Append(ch);
							break;
					}
				}
			}

			var semver = new SemanticVersion();

			foreach (var buffer in buffers)
			{
				buffer.Item3(semver, buffer.Item2.ToString());
			}

			return semver;
		}

		public static SemanticVersion CreateFrom(
			SemanticVersion version,
			int? major = null,
			int? minor = null,
			int? patch = null,
			string prereleaseTag = null,
			string metadata = null
		)
		{
			var actualMajor = major ?? version.Major;
			var actualMinor = minor ?? version.Minor;
			var actualPatch = patch ?? version.Patch;
			var actualPrereleseTag = prereleaseTag ?? version.PrereleaseTag;
			var actualMetadata = metadata ?? version.Metadata;

			return Create(actualMajor, actualMinor, actualPatch, actualPrereleseTag, actualMetadata);

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

		public SemanticVersion IncrementMinor()
		{
			return new SemanticVersion(this.Major, this.Minor++, 0);
		}
		public SemanticVersion IncrementMajor()
		{
			return new SemanticVersion(this.Major++, 0, 0);
		}

		private enum ParsingMode
		{
			Numeric,
			Tag,
			Metadata
		}
	}
}