using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Variables
{
	public class VariableRenderer : IVariableRenderer
	{
		private readonly IVariableStore _variableStore;
		public VariableRenderer(
			IVariableStore variableStore
		)
		{
			_variableStore = variableStore;			
		}
		public string Render(
			string template,
			Project project
		)
		{
			var globalVariables = _variableStore.Global.GetVariables();
			var projecVariables = _variableStore.ProjectVariables[project].GetVariables();
			var variables = globalVariables.Union(projecVariables).ToDictionary(x => x.Key, x => x.Value);
			
			foreach (var pair in variables)
			{
				template = template.Replace($"{{{pair.Key}}}", pair.Value);
			}

			return template;
		}
	}
}