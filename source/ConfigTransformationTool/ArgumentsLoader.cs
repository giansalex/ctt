// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace ConfigTransformationTool
{
    using System;

    /// <summary>
    /// Command line helper. Load parameters from comman line arguments.
    /// </summary>
    internal class ArgumentsLoader
    {
        public bool IsAllRequiredParametersSet
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.SourceFilePath)
                    && !string.IsNullOrWhiteSpace(this.TransformFilePath)
                    && !string.IsNullOrWhiteSpace(this.DestinationFilePath);
            }
        }

        public string SourceFilePath { get; private set; }

        public string TransformFilePath { get; private set; }

        public string DestinationFilePath { get; private set; }

        public string ParametersString { get; private set; }

        public string ParametersFile { get; private set; }

        public bool ForceParametersTask { get; private set; }

        /// <summary>
        /// Load arguments from command line
        /// </summary>
        /// <param name="args"></param>
        public void Load(string[] args)
        {
            this.DestinationFilePath = string.Empty;
            this.SourceFilePath = string.Empty;
            this.TransformFilePath = string.Empty;
            this.ParametersString = string.Empty;
            this.ParametersFile = string.Empty;
            this.ForceParametersTask = false;

            foreach (string arg in args)
            {
                if (arg.StartsWith("s:") || arg.StartsWith("source:"))
                {
                    this.SourceFilePath = GetFileNameFromArguments(arg);
                }

                if (arg.StartsWith("t:") || arg.StartsWith("transform:"))
                {
                    this.TransformFilePath = GetFileNameFromArguments(arg);
                }

                if (arg.StartsWith("d:") || arg.StartsWith("destination:"))
                {
                    this.DestinationFilePath = GetFileNameFromArguments(arg);
                }

                if (arg.StartsWith("p:") || arg.StartsWith("parameters:"))
                {
                    this.ParametersString = GetValueFromArguments(arg);
                }

                if (arg.StartsWith("pf:") || arg.StartsWith("parameters.file:"))
                {
                    this.ParametersFile = GetValueFromArguments(arg);
                }

                if (arg.StartsWith("fpt"))
                {
                    this.ForceParametersTask = true;
                }
            }
        }

        private static string GetFileNameFromArguments(string arg)
        {
            return GetValueFromArguments(arg).Trim('"');
        }

        private static string GetValueFromArguments(string arg)
        {
            int startIndex = arg.IndexOf(":", StringComparison.Ordinal) + 1;
            return arg.Substring(startIndex, arg.Length - startIndex);
        }
    }
}