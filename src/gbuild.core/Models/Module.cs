namespace GBuild.Core.Models
{
	public class Module
	{
		public Module(
			string name
		)
		{
			Name = name;

		}

		public string Name { get; }

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

			return Equals((Module) obj);
		}

		public override int GetHashCode()
		{
			return Name != null ? Name.GetHashCode() : 0;
		}
	}
}