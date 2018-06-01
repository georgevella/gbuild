using System;
using System.Linq;
using System.Linq.Expressions;
using LibGit2Sharp;

namespace GBuild.Core.CommitAnalysis.Git
{
	public class Commit
	{
		private Commit(
			Signature cAuthor,
			string cMessage,
			Signature cCommitter
		)
		{
			throw new System.NotImplementedException();
		}

		public static implicit operator Commit(
			LibGit2Sharp.Commit c
		)
		{
			return new Commit(
				c.Author,
				c.Message,
				c.Committer				
				);
		}
	}

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