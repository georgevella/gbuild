using System;

namespace GBuild.Core.Models
{
	public class Commit
	{
		public Commit(
			string message,
			string id,
			string committer,
			DateTimeOffset when
		)
		{
			Message = message;
			Id = id;
			Committer = committer;
			When = when;
		}

		public string Message { get; }
		public string Id { get; }
		public string Committer { get; }
		public DateTimeOffset When { get; }
	}
}