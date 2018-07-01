using System.IO;

namespace GBuild.Workspaces
{
	public interface IWorkspaceRootDirectoryProvider
	{
		DirectoryInfo GetWorkspaceRootDirectory();
	}
}