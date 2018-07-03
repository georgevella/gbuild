using System.Collections.Concurrent;
using System.Collections.Generic;
using Serilog;

namespace GBuild.Variables
{
	internal class VariableCollection : IVariableCollection
	{
		private readonly ConcurrentDictionary<string, string> _variables = new ConcurrentDictionary<string, string>();
		public string this[
			string name
		]
		{
			get => _variables.TryGetValue(name, out var value) ? value : null;
			set
			{
				_variables.AddOrUpdate(
					name,
					key =>
					{
						Log.Verbose("Adding variable {variable} with value: {value}", key, value);
						return value;
					}, 
					(
						key,
						currentValue
					) =>
					{
						if (currentValue != value)
						{
							Log.Verbose("Updating variable {variable} with value: {value}", key, value);
							return value;
						}

						Log.Verbose("Variable {variable} unchanged.");
						return currentValue;
					});
			}
		}

		public IEnumerable<KeyValuePair<string, string>> GetVariables()
		{
			return _variables;
		}
	}
}