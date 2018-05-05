using System.Collections.Generic;
using GBuild.Core.VcsSupport;

namespace GBuild.Core.Context.Data
{
    public class BranchInformation
    {
        public BranchInformation(Branch currentBranch, IEnumerable<Branch> allBranches)
        {
            CurrentBranch = currentBranch;
            Branches = allBranches;
        }

        public Branch CurrentBranch { get; }

        public IEnumerable<Branch> Branches { get; }
    }
}