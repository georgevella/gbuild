namespace GBuild.Models
{
	public class ChangedFile
	{
		protected bool Equals(
			ChangedFile other
		)
		{
			return string.Equals(Path, other.Path);
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

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((ChangedFile) obj);
		}

		public override int GetHashCode()
		{
			return (Path != null ? Path.GetHashCode() : 0);
		}

		public ChangedFile(string path)
		{
			Path = path;
		}

		public string Path { get; }
	}
}