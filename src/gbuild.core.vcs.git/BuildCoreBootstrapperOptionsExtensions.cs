using System.Reflection;

namespace GBuild.Core.Vcs.Git
{
	public static class BuildCoreBootstrapperOptionsExtensions
	{
		public static BuildCoreBootsrapperOptions UseGit(
			this BuildCoreBootsrapperOptions options
		)
		{
			options.Assemblies.Add(Assembly.GetExecutingAssembly());

			options.RepositoryType = typeof(GitSourceCodeRespository);

			return options;
		}
	}
}