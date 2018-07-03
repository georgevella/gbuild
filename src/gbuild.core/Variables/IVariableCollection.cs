using System.Collections.Generic;

namespace GBuild.Variables
{
	public interface IVariableCollection
	{
		string this[string name] { get; set; }

		IEnumerable<KeyValuePair<string, string>> GetVariables();
	}
}