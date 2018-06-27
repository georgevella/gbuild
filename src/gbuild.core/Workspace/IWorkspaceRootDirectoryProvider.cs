using System.IO;

namespace GBuild.Workspace
{
	public interface IWorkspaceRootDirectoryProvider
	{
		DirectoryInfo GetWorkspaceRootDirectory();
	}
}