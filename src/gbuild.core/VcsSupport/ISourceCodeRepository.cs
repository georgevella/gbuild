using System.Collections.Generic;
using GBuild.Core.Models;

namespace GBuild.Core.VcsSupport
{
	public interface ISourceCodeRepository
	{
		Branch CurrentBranch { get; }
		IEnumerable<Branch> Branches { get; }
		IEnumerable<Tag> Tags { get; }

		IEnumerable<Commit> GetCommitsBetween(
			Branch parentBranch,
			Branch branch
		);

		IEnumerable<ChangedFile> GetFilesChangedBetween(
			Branch parentBranch,
			Branch branch
		);
	}
}