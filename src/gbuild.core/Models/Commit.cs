using System.Collections.Generic;

namespace GBuild.Core.Models
{
	public class Commit
	{
		protected bool Equals(Commit other)
		{
			return string.Equals(Id, other.Id);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((Commit) obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return (Id != null ? Id.GetHashCode() : 0);
		}

		public Commit(string id, string committer, string message, IEnumerable<ChangedFile> changedFiles)
		{
			Committer = committer;
			Message = message;
			ChangedFiles = changedFiles;
			Id = id;
		}

		public string Id { get; }

		public string Committer { get; }
		public string Message { get; }

		public IEnumerable<ChangedFile> ChangedFiles { get; }
	}
}