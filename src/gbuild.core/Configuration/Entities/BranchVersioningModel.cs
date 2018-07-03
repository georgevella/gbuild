namespace GBuild.Configuration.Entities
{
	public class BranchVersioningModel
	{
		public BranchVersioningModel()
		{
			Tag = string.Empty;
			Metadata = string.Empty;
		}		

		public string Tag { get; set; }

		public string Metadata { get; set; }

		public VersionIncrementStrategy Increment { get; set; }
	}
}