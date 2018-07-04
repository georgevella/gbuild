using System;
using System.Collections.Generic;
using System.Linq;
using GBuild.Context;
using GBuild.Models;

namespace GBuild.Variables
{
	public class VariableRenderer : IVariableRenderer
	{
		public string Render(
			string template,
			Project project,
			IVariableStore variableStore
		)
		{
			var globalVariables = variableStore.Global.GetVariables();
			var projecVariables = variableStore.ProjectVariables[project].GetVariables();
			var variables = globalVariables.Union(projecVariables).ToDictionary(x => x.Key, x => x.Value);
			
			foreach (var pair in variables)
			{
				template = template.Replace($"{{{pair.Key}}}", pair.Value);
			}

			return template;
		}
	}
}