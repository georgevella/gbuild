using System.Collections.Generic;
using System.IO;

namespace GBuild.Core.Context.Data
{
    public class SourceCodeInformation
    {
        public SourceCodeInformation(DirectoryInfo repositoryRootDirectory, DirectoryInfo sourceCodeRootDirectory, IEnumerable<Project> projects)
        {
            RepositoryRootDirectory = repositoryRootDirectory;
            Projects = projects;
            SourceCodeRootDirectory = sourceCodeRootDirectory;
        }

        public DirectoryInfo RepositoryRootDirectory { get; }

        public DirectoryInfo SourceCodeRootDirectory { get; }

        public IEnumerable<Project> Projects { get; }
    }

    public class Project
    {
        public Project(string name, FileInfo file, ProjectType type)
        {
            Name = name;
            File = file;
            Type = type;
        }

        public string Name { get; }

        public FileInfo File { get; }

        public ProjectType Type { get; }
    }

    public enum ProjectType
    {
        LegacyCSharp,
        CSharp,
    }
}