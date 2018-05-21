using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Core.Context.Data;
using GBuild.Core.Models;
using GBuild.Core.Vcs;

namespace GBuild.Core.Context.Providers
{
	public class CommitAnalysisContextDataProvider : IContextDataProvider<CommitAnalysis>
	{
		private readonly IContextData<BranchInformation> _branchInformation;
		private readonly IContextData<ProjectInformation> _projectInformation;
		private readonly ISourceCodeRepository _sourceCodeRepository;

		public CommitAnalysisContextDataProvider(
			ISourceCodeRepository sourceCodeRepository,
			IContextData<BranchInformation> branchInformation,
			IContextData<ProjectInformation> projectInformation
		)
		{
			_sourceCodeRepository = sourceCodeRepository;
			_branchInformation = branchInformation;
			_projectInformation = projectInformation;
		}

		public CommitAnalysis LoadContextData()
		{
			var branchVersioningStrategy = _branchInformation.Data.VersioningStrategyModel;

			// TODO: how to handle branches that are not development / slaves of other branches

			var parentBranch = _sourceCodeRepository.Branches.First(b => b.Name == branchVersioningStrategy.ParentBranch);

			var commits = _sourceCodeRepository.GetCommitsBetween(
				parentBranch,
				_sourceCodeRepository.CurrentBranch
			);

			// determine changed files
			var files = _sourceCodeRepository.GetFilesChangedBetween(
				parentBranch,
				_sourceCodeRepository.CurrentBranch
			);

			// determine changed modules
			var rootDirectory = new Uri(_projectInformation.Data.RepositoryRootDirectory.FullName.TrimEnd('\\') + "\\");

			var moduleRootDirectories = _projectInformation.Data.Modules.OfType<CsharpModule>()
				.Select(m => new 
					{
						Module = m,
						Uri = rootDirectory.MakeRelativeUri(new Uri(m.File.DirectoryName))
					}
				)
				.Select(m => new
				{
					m.Module,
					Path = Uri.UnescapeDataString(m.Uri.ToString())
				})
				.ToDictionary(m => m.Path, m => m.Module);

			var changedModules = new List<Module>();

			foreach (var file in files)
			foreach (var rootDir in moduleRootDirectories)
			{
				if (file.Path.StartsWith(rootDir.Key, StringComparison.OrdinalIgnoreCase) &&
					!changedModules.Contains(rootDir.Value))
				{
					changedModules.Add(rootDir.Value);
				}
			}

			return new CommitAnalysis(
				changedModules,
				commits,
				false,
				false
			);
		}
	}
}