using GBuild.Configuration.Entities;

namespace GBuild.Configuration.Models
{
	public interface IBranchVersioningSettings
	{
		string Tag { get; set; }
		string Metadata { get; set; }
		VersionIncrementStrategy Increment { get; set; }
	}
}