using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GBuild.Models;

namespace GBuild.Variables
{
	public interface IVariableStore
	{
		IVariableCollection Global { get; }
		IReadOnlyDictionary<Project, IVariableCollection> ProjectVariables { get; }
		void AddProject(Project project);
	}
}