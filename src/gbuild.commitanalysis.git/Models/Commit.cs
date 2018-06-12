namespace gbuild.commitanalysis.git.Models
{
	public class Commit
	{
		public string Id { get; }
		public Signature Author { get; }
		public Signature Committer { get; }
		public string Message { get; }

		private Commit(
			string id,
			Signature author,			
			Signature committer,
			string message
		)
		{
			Id = id;
			Author = author;
			Committer = committer;
			Message = message;
		}

		public static implicit operator Commit(
			LibGit2Sharp.Commit c
		)

		{
			return new Commit(
				c.Id.Sha,
				c.Author,
				c.Committer,
				c.Message
			);
		}
	}
}