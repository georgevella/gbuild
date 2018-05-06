namespace GBuild.Core.Configuration
{
    public class BranchVersioningStrategy
    {
        public BranchVersioningStrategy()
        {
            Tag = string.Empty;
            Metadata = string.Empty;
        }
        /// <summary>
        ///     Canonical name or pattern for one or more branches.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Another branch in the repository that will be tracked for version information.
        /// </summary>
        public string ParentBranch { get; set; }

        /// <summary>
        ///     Pre-release tag.
        /// </summary>
        public string Tag { get; set; }

        public string Metadata { get; set; }
    }
}