// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Parse string of parameters 
    /// </summary>
    public class ParametersParser
    {
        private readonly OutputLog _log;

        public ParametersParser(OutputLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Parse string of parameters <paramref name="parametersString"/> separated by semi ';'.
        /// Value should be separated from name by colon ':'. 
        /// If value has spaces or semi you can use quotes for value. 
        /// You can escape symbols '\' and '"' with \.
        /// </summary>
        /// <param name="parametersString">String of parameters</param>
        /// <param name="parameters">All parameters will be read to current dictionary.</param>
        public void ReadParameters(string parametersString, IDictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (string.IsNullOrWhiteSpace(parametersString))
            {
                return;
            }

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
                    || (fForceParameterValueRead && source[index] == '"'
                        && ((index + 1) == source.Length || source[index + 1] == ';')))
                {
                    AddParameter(parameters, parameterName, parameterValue);
                    index++;
                    if (fForceParameterValueRead)
                    {
                        index++;
                    }

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

            foreach (var parameter in parameters)
            {
                _log.WriteLine("Parameter Name: '{0}', Value: '{1}'", parameter.Key, parameter.Value);
            }
        }

        private static void AddParameter(
            IDictionary<string, string> parameters, StringBuilder parameterName, StringBuilder parameterValue)
        {
            var name = parameterName.ToString();
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (parameters.ContainsKey(name))
                {
                    parameters.Remove(name);
                }

                parameters.Add(name, parameterValue.ToString());
            }
        }
    }
}