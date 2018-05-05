using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Core.Context;
using GBuild.Core.Context.Data;
using LibGit2Sharp;

namespace GBuild.Core.VcsSupport.Git
{
    public class GitSourceCodeRespository : ISourceCodeRepository, IDisposable
    {
        private readonly Repository _repository;

        public GitSourceCodeRespository(IContextData<SourceCodeInformation> sourceCodeInformation)
        {
            _repository = new Repository(sourceCodeInformation.Data.RepositoryRootDirectory.FullName);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public Branch CurrentBranch
        {
            get
            {
                var currentBranch = _repository.Branches.First(b => b.IsCurrentRepositoryHead);
                return Convert(currentBranch);
            }
        }

        public IEnumerable<Branch> Branches => _repository.Branches.Select(Convert).ToList();
        public IEnumerable<Tag> Tags => _repository.Tags.Select(Convert).ToList();

        public IEnumerable<Revision> GetCommitsBetween(Branch parentBranch, Branch branch)
        {
            var filter = new CommitFilter
            {
                ExcludeReachableFrom = _repository.Branches[parentBranch.Name],
                IncludeReachableFrom = _repository.Branches[branch.Name],
                SortBy = CommitSortStrategies.Time
            };

            return _repository.Commits.QueryBy(filter).Select(Convert).ToList();
        }

        private static Branch Convert(LibGit2Sharp.Branch branch)
        {
            return new Branch(branch.CanonicalName, Convert(branch.Tip));
        }

        private static Revision Convert(Commit commit)
        {
            return new Revision(
                commit.Message,
                commit.Sha,
                commit.Committer.Email,
                commit.Committer.When
            );
        }

        private Tag Convert(LibGit2Sharp.Tag tag)
        {
            return new Tag(tag.CanonicalName, Convert((Commit) tag.Target));
        }
    }
}