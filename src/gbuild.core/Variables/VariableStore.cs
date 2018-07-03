using System.Collections.Generic;
using System.Linq;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Variables
{
	internal class VariableStore : IVariableStore
	{
		private readonly Dictionary<Project, IVariableCollection> _projectVariables = new Dictionary<Project, IVariableCollection>();
		public IVariableCollection Global { get; } = new VariableCollection();
		public IReadOnlyDictionary<Project, IVariableCollection> ProjectVariables => _projectVariables;
		public void AddProject(Project project) => _projectVariables.Add(project, new VariableCollection());
	}
}