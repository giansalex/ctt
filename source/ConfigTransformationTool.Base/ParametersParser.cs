using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;

namespace ConfigTransformationTool.Base
{
	/// <summary>
	/// Parse string of parameters 
	/// </summary>
	public static class ParametersParser
	{
		private readonly static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); 

		/// <summary>
		/// Parse string of parameters <paramref name="parametersString"/> separated by semi ';'.
		/// Value should be separated from name by colon ':'. 
		/// If value has spaces or semi you can use quotes for value. 
		/// You can escape symbols '\' and '"' with \.
		/// </summary>
		/// <param name="parametersString">String of parameters</param>
		/// <returns>Dicrionary of parameters, where keys are names and values are values of parameters. 
		/// Can be null if <paramref name="parametersString"/> is empty or null.</returns>
		public static IDictionary<string, string> ReadParameters(string parametersString)
		{
			if (string.IsNullOrWhiteSpace(parametersString)) return null;

			Dictionary<string, string> parameters = new Dictionary<string, string>();

			var source = parametersString.ToCharArray();

			int index = 0;

			bool fParameterNameRead = true;
			bool fForceParameterValueRead = false;

			StringBuilder parameterName = new StringBuilder();
			StringBuilder parameterValue = new StringBuilder();

			while (index < source.Length)
			{
				if (fParameterNameRead && source[index] == ':')
				{
					fParameterNameRead = false;
					index++;

					if (index < source.Length && source[index] == '"')
					{
						fForceParameterValueRead = true;
						index++;
					}

					continue;
				}

				if ((!fForceParameterValueRead && source[index] == ';')
					|| (fForceParameterValueRead && source[index] == '"' && ((index + 1) == source.Length || source[index + 1] == ';')))
				{
					AddParameter(parameters, parameterName, parameterValue);
					index++;
					if (fForceParameterValueRead)
						index++;
					parameterName.Clear();
					parameterValue.Clear();
					fParameterNameRead = true;
					fForceParameterValueRead = false;
					continue;
				}

				// Check is this escape \{ \} \\
				if (source[index] == '\\')
				{
					var nextIndex = index + 1;
					if (nextIndex < source.Length)
					{
						var nextChar = source[nextIndex];
						if (nextChar == '"' || nextChar == '\\')
						{
							index++;
						}
					}
				}

				if (fParameterNameRead)
				{
					parameterName.Append(source[index]);
				}
				else
				{
					parameterValue.Append(source[index]);
				}

				index++;
			}

			AddParameter(parameters, parameterName, parameterValue);

			if (Log.IsDebugEnabled)
			{
				foreach (var parameter in parameters)
				{
					Log.DebugFormat("Parameter Name: '{0}', Value: '{1}'", parameter.Key, parameter.Value);
				}
			}

			return parameters;
		}

		private static void AddParameter(Dictionary<string, string> parameters, StringBuilder parameterName, StringBuilder parameterValue)
		{
			var name = parameterName.ToString();
			if (!string.IsNullOrWhiteSpace(name))
			{
				if (parameters.ContainsKey(name))
					parameters.Remove(name);
				parameters.Add(name, parameterValue.ToString());
			}
		}
	}
}
