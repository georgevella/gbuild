using System.IO;

namespace GBuild.Core.Models
{
	public class Module
	{
		public Module(
			string name,
			FileInfo file,
			ModuleType type
		)
		{
			Name = name;
			File = file;
			Type = type;
		}

		public string Name { get; }

		public FileInfo File { get; }

		public ModuleType Type { get; }

		protected bool Equals(
			Module other
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

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((Module) obj);
		}

		public override int GetHashCode()
		{
			return Name != null ? Name.GetHashCode() : 0;
		}
	}
}