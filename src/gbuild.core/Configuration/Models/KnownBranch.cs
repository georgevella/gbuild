using System.Text.RegularExpressions;

namespace GBuild.Configuration.Models
{
	internal class KnownBranch : IKnownBranch
	{
		private readonly string _pattern;
		public KnownBranch(
			string name,
			string pattern,
			BranchType type,
			IBranchVersioningStrategy versioningStrategy
		)
		{
			_pattern = pattern;
			Name = name;
			Type = type;
			VersioningStrategy = versioningStrategy;
		}

		public string Name { get; }
		public IBranchVersioningStrategy VersioningStrategy { get; }
		public BranchType Type { get; }
		public bool IsMatch(
			string branchName
		)
		{
			if (branchName == _pattern)
			{
				return true;
			}

			try
			{
				if (Regex.IsMatch(branchName, _pattern))
				{
					return true;
				}
			}
			catch
			{

			}

			// TODO: pattern matching branch name
			return false;
		}
	}
}