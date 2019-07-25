using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSource.ConfigTransformationTool
{

    /// <summary>
    /// Task replace parameters with values
    /// </summary>
    public class ParametersTask
    {
        private IDictionary<string, string> _parameters;

        /// <summary>
        /// Set parameters
        /// </summary>
        /// <param name="parameters">Dictionary of parameters with values.</param>
        public void AddParameters(IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (_parameters == null)
            {
                _parameters = new Dictionary<string, string>(parameters);
            }
            else
            {
                foreach (var parameter in parameters)
                {
                    _parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// Apply parameters to <paramref name="sourceString"/>
        /// </summary>
        /// <param name="sourceString">Source string</param>
        /// <returns>String with set values instead of parameters</returns>
        public string ApplyParameters(string sourceString)
        {
            var result = new StringBuilder();

            var index = 0;

            var source = sourceString.ToCharArray();

            var fParameterRead = false;

            var parameter = new StringBuilder();

            while (index < source.Length)
            {
                // If parameter read, read it and replace it
                if (fParameterRead && source[index] == '}')
                {
                    var s = parameter.ToString();
                    var colonIndex = parameter.ToString().IndexOf(':');

                    var parameterName = colonIndex > 0 ? s.Substring(0, colonIndex) : s;
                    var parameterDefaultValue = colonIndex > 0
                                                    ? s.Substring(colonIndex + 1, s.Length - colonIndex - 1)
                                                    : null;

                    string parameterValue = null;
                    if (_parameters != null && _parameters.ContainsKey(parameterName))
                    {
                        parameterValue = _parameters[parameterName];
                    }

                    // Put "value" or "default value" or "string which was here"
                    result.Append(parameterValue ?? parameterDefaultValue ?? "{" + parameter + "}");

                    fParameterRead = false;
                    index++;
                    continue;
                }

                if (source[index] == '{')
                {
                    fParameterRead = true;
                    parameter = new StringBuilder();
                    index++;
                }
                else if (source[index] == '\\')
                {
                    // Check is this escape \{ \} \\
                    var nextIndex = index + 1;
                    if (nextIndex < source.Length)
                    {
                        var nextChar = source[nextIndex];
                        if (nextChar == '}' || nextChar == '{' || nextChar == '\\')
                        {
                            index++;
                        }
                    }
                }

                if (fParameterRead)
                {
                    parameter.Append(source[index]);
                }
                else
                {
                    result.Append(source[index]);
                }

                index++;
            }

            return result.ToString();
        }
    }
}
