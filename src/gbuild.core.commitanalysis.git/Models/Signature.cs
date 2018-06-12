using System;

namespace GBuild.Core.CommitAnalysis.Git.Models
{
	public class Signature
	{
		public string Name { get; }
		public string Email { get; }
		public DateTimeOffset When { get; }

		public Signature(
			string name,
			string email,
			DateTimeOffset @when
		)
		{
			Name = name;
			Email = email;
			When = when;
		}

		public static implicit operator Signature(
			LibGit2Sharp.Signature s
		)
		{
			return new Signature(s.Name, s.Email, s.When);
		}
	}
}