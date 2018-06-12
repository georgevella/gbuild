namespace gbuild.commitanalysis.git.Models
{
	public class Branch
	{
		public bool IsCurrentRepositoryHead { get; }
		public string CanonicalName { get; }

		public Branch(
			string canonicalName,
			bool isCurrentRepositoryHead
		)
		{
			CanonicalName = canonicalName;
			IsCurrentRepositoryHead = isCurrentRepositoryHead;
		}

		public static explicit operator Branch(
			LibGit2Sharp.Branch source
		)
		{
			return new Branch(source.CanonicalName, source.IsCurrentRepositoryHead);
		}
	}
}