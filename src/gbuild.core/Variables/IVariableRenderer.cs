using GBuild.Models;

namespace GBuild.Variables
{
	public interface IVariableRenderer
	{
		string Render(
			string template,
			Project project,
			IVariableStore variableStore
		);
	}
}