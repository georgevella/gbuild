using GBuild.Configuration.Entities;

namespace GBuild.Configuration
{
	public interface IConfigurationFileLoader
	{
		ConfigurationFile Load();
	}
}