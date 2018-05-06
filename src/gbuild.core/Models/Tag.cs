using GBuild.Core.VcsSupport;

namespace GBuild.Core.Models
{
    public class Tag
    {
        public Tag(string canonicalName, Commit commit)
        {
            CanonicalName = canonicalName;
            Commit = commit;
        }

        public string CanonicalName { get; }
        public Commit Commit { get; }
    }
}