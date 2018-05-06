namespace GBuild.Core.Models
{
	public class Branch
	{
		public Branch(
			string name,
			Commit latestCommit
		)
		{
			Name = name;
			LatestCommit = latestCommit;
		}

		public string Name { get; }
		public Commit LatestCommit { get; }
	}
}