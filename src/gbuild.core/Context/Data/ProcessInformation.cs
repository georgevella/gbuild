using System.IO;

namespace GBuild.Core.Context.Data
{
    public class ProcessInformation
    {
        public ProcessInformation(DirectoryInfo currentDirectory)
        {
            CurrentDirectory = currentDirectory;
        }

        public DirectoryInfo CurrentDirectory { get; }
    }
}