using System.Collections.Generic;
using System.IO;
using GBuild.Context;
using GBuild.Models;
using LibGit2Sharp;
using Commit = LibGit2Sharp.Commit;

namespace GBuild
{
	public class RepositoryWrapper : IRepository
	{
		private readonly IRepository _repository;

		private static string GetRepositoryRootDirectory(IContextData<Process> processContextData)
		{
			var repositoryRootDirectory = processContextData.Data.CurrentDirectory;
			var dotGitDirectory = new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));

			while (!dotGitDirectory.Exists && repositoryRootDirectory.Parent != null)
			{
				repositoryRootDirectory = repositoryRootDirectory.Parent;
				dotGitDirectory = new DirectoryInfo(Path.Combine(repositoryRootDirectory.FullName, ".git"));
			}

			return repositoryRootDirectory.FullName;
		}

		public RepositoryWrapper(IContextData<Process> processContextData)
		{
			_repository = new Repository(GetRepositoryRootDirectory(processContextData));
		}

		public void Dispose()
		{
			_repository.Dispose();
		}

		public void Checkout(
			Tree tree,
			IEnumerable<string> paths,
			CheckoutOptions opts
		)
		{
			_repository.Checkout(tree, paths, opts);
		}

		public void CheckoutPaths(
			string committishOrBranchSpec,
			IEnumerable<string> paths,
			CheckoutOptions checkoutOptions
		)
		{
			_repository.CheckoutPaths(committishOrBranchSpec, paths, checkoutOptions);
		}

		public GitObject Lookup(
			ObjectId id
		)
		{
			return _repository.Lookup(id);
		}

		public GitObject Lookup(
			string objectish
		)
		{
			return _repository.Lookup(objectish);
		}

		public GitObject Lookup(
			ObjectId id,
			ObjectType type
		)
		{
			return _repository.Lookup(id, type);
		}

		public GitObject Lookup(
			string objectish,
			ObjectType type
		)
		{
			return _repository.Lookup(objectish, type);
		}

		public LibGit2Sharp.Commit Commit(
			string message,
			Signature author,
			Signature committer,
			CommitOptions options
		)
		{
			return _repository.Commit(message, author, committer, options);
		}

		public void Reset(
			ResetMode resetMode,
			Commit commit
		)
		{
			_repository.Reset(resetMode, commit);
		}

		public void Reset(
			ResetMode resetMode,
			Commit commit,
			CheckoutOptions options
		)
		{
			_repository.Reset(resetMode, commit, options);
		}

		public void RemoveUntrackedFiles()
		{
			_repository.RemoveUntrackedFiles();
		}

		public RevertResult Revert(
			Commit commit,
			Signature reverter,
			RevertOptions options
		)
		{
			return _repository.Revert(commit, reverter, options);
		}

		public MergeResult Merge(
			Commit commit,
			Signature merger,
			MergeOptions options
		)
		{
			return _repository.Merge(commit, merger, options);
		}

		public MergeResult Merge(
			Branch branch,
			Signature merger,
			MergeOptions options
		)
		{
			return _repository.Merge(branch, merger, options);
		}

		public MergeResult Merge(
			string committish,
			Signature merger,
			MergeOptions options
		)
		{
			return _repository.Merge(committish, merger, options);
		}

		public MergeResult MergeFetchedRefs(
			Signature merger,
			MergeOptions options
		)
		{
			return _repository.MergeFetchedRefs(merger, options);
		}

		public CherryPickResult CherryPick(
			Commit commit,
			Signature committer,
			CherryPickOptions options
		)
		{
			return _repository.CherryPick(commit, committer, options);
		}

		public BlameHunkCollection Blame(
			string path,
			BlameOptions options
		)
		{
			return _repository.Blame(path, options);
		}

		public FileStatus RetrieveStatus(
			string filePath
		)
		{
			return _repository.RetrieveStatus(filePath);
		}

		public RepositoryStatus RetrieveStatus(
			StatusOptions options
		)
		{
			return _repository.RetrieveStatus(options);
		}

		public string Describe(
			Commit commit,
			DescribeOptions options
		)
		{
			return _repository.Describe(commit, options);
		}

		public void RevParse(
			string revision,
			out Reference reference,
			out GitObject obj
		)
		{
			_repository.RevParse(revision, out reference, out obj);
		}

		public Branch Head => _repository.Head;

		public LibGit2Sharp.Configuration Config => _repository.Config;

		public Index Index => _repository.Index;

		public ReferenceCollection Refs => _repository.Refs;

		public IQueryableCommitLog Commits => _repository.Commits;

		public BranchCollection Branches => _repository.Branches;

		public TagCollection Tags => _repository.Tags;

		public RepositoryInformation Info => _repository.Info;

		public Diff Diff => _repository.Diff;

		public ObjectDatabase ObjectDatabase => _repository.ObjectDatabase;

		public NoteCollection Notes => _repository.Notes;

		public SubmoduleCollection Submodules => _repository.Submodules;

		public Rebase Rebase => _repository.Rebase;

		public Ignore Ignore => _repository.Ignore;

		public Network Network => _repository.Network;

		public StashCollection Stashes => _repository.Stashes;
	}
}