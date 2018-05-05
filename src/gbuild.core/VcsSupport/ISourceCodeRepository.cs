using System.Collections.Generic;

namespace GBuild.Core.VcsSupport
{
    public interface ISourceCodeRepository
    {
        Branch CurrentBranch { get; }
        IEnumerable<Branch> Branches { get; }
        IEnumerable<Tag> Tags { get; }
        IEnumerable<Revision> GetCommitsBetween(Branch parentBranch, Branch branch);
    }

    public class Tag
    {
        public Tag(string canonicalName, Revision revision)
        {
            CanonicalName = canonicalName;
            Revision = revision;
        }

        public string CanonicalName { get; }
        public Revision Revision { get; }
    }

    public class Branch
    {
        public Branch(string name, Revision latestRevision)
        {
            Name = name;
            LatestRevision = latestRevision;
        }

        public string Name { get; }
        public Revision LatestRevision { get; }
    }
}