using System.Collections.Generic;

namespace GBuild.Core.Configuration
{
    public class ConfigurationFile
    {
        /// <summary>
        ///     Relative path to the location of all sources.
        /// </summary>
        public string SourceCodeRoot { get; set; }

        public BranchingModelType BranchingModel { get; set; } 

        public List<BranchStrategy> Branches { get; } = new List<BranchStrategy>();
    }

    public enum BranchingModelType
    {
        GitFlow,
        GitHubFlow,
        TrunkBased
    }

    public class BranchStrategy
    {
        /// <summary>
        ///     Canonical name or pattern for one or more branches.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        ///     Another branch in the repository that will be tracked for version information.
        /// </summary>
        public string ParentBranch { get; set; }        
    }
}