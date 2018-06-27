using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Variables
{
	public class VariableRenderer : IVariableRenderer
	{
		private readonly IContextData<WorkspaceDescription> _workspaceInformation;
		private readonly IContextData<CommitHistoryAnalysis> _commitHistoryAnalysis;

		public VariableRenderer(
			IContextData<WorkspaceDescription> workspaceInformation,
			IContextData<CommitHistoryAnalysis> commitHistoryAnalysis
		)
		{
			_workspaceInformation = workspaceInformation;
			_commitHistoryAnalysis = commitHistoryAnalysis;
		}

		public string Render(
			string template,
			Project project
		)
		{
			var variables = _workspaceInformation.Data.Variables.ToDictionary(x => x.Key, x => x.Value);
			variables[ProjectVariables.CommitCount] = _commitHistoryAnalysis.Data.ChangedProjects[project].Commits.Count.ToString();
			
			foreach (var pair in variables)
			{
				template = template.Replace($"{{{pair.Key}}}", pair.Value);
			}

			return template;
		}
	}

	public interface IVariableRenderer
	{
		string Render(
			string template,
			Project project
		);
	}
}