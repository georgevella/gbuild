using System.IO;

namespace GBuild.Core.Models
{
	public class Project
	{
		public Project(
			string name,
			DirectoryInfo path
		)
		{
			Name = name;
			Path = path;

		}

		public string Name { get; }

		public DirectoryInfo Path { get; }

		protected bool Equals(
			Project other
		)
		{
			return string.Equals(Name, other.Name);
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

			return Equals((Project) obj);
		}

		public override int GetHashCode()
		{
			return Name != null ? Name.GetHashCode() : 0;
		}
	}
}