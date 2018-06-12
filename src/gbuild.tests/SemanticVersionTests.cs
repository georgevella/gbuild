using System.Collections.Generic;
using FluentAssertions;
using GBuild;
using Xunit;

namespace gbuild.tests
{
	public class SemanticVersionTests
	{
		[Theory]
		[MemberData(nameof(Data))]
		public void ParsingTests(string stringVersion, SemanticVersion expected)
		{
			var actual = SemanticVersion.Parse(stringVersion);

			actual.Major.Should().Be(expected.Major);
			actual.Minor.Should().Be(expected.Minor);
			actual.Patch.Should().Be(expected.Patch);
			actual.PrereleaseTag.Should().Be(expected.PrereleaseTag);
			actual.Metadata.Should().Be(expected.Metadata);

			actual.Should().Be(expected);
		}

		public static IEnumerable<object[]> Data => new List<object[]>()
		{
			new object[] {"1.0.0", new SemanticVersion(1, 0, 0)},
			new object[] {"1.0.0-tag", new SemanticVersion(1, 0, 0, "tag")},
			new object[] {"1.0.0-tag+metadata", new SemanticVersion(1, 0, 0, "tag", "metadata")},
			new object[] {"1.0.0-tag.with.dot+metadata", new SemanticVersion(1, 0, 0, "tag.with.dot", "metadata")},
			new object[]
				{"1.0.0-tag.with.dot+metadata.with.dot", new SemanticVersion(1, 0, 0, "tag.with.dot", "metadata.with.dot")},
			new object[]
				{"1.0.0-tag123+metadata.with.dot-123", new SemanticVersion(1, 0, 0, "tag123", "metadata.with.dot-123")},
		};
	}
}