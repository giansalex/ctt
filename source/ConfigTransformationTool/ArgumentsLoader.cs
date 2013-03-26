// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;

    /// <summary>
    /// Command line helper. Load parameters from comman line arguments.
    /// </summary>
    internal class ArgumentsLoader
    {
        public bool AreAllRequiredParametersSet
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

        public bool Verbose { get; private set; }

        public bool PreserveWhitespace { get; private set; }

        public bool Indent { get; private set; }

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
            this.Verbose = false;
            this.PreserveWhitespace = false;
            this.Indent = false;

            foreach (string arg in args)
            {
                if (arg.IndexOf("s:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("source:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.SourceFilePath = GetValueFromArguments(arg);
                }

                if (arg.IndexOf("t:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("transform:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.TransformFilePath = GetValueFromArguments(arg);
                }

                if (arg.IndexOf("d:", StringComparison.OrdinalIgnoreCase) == 0 
                    || arg.IndexOf("destination:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.DestinationFilePath = GetValueFromArguments(arg);
                }

                if (arg.IndexOf("p:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("parameters:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.ParametersString = GetValueFromArguments(arg);
                }

                if (arg.IndexOf("pf:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("parameters.file:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.ParametersFile = GetValueFromArguments(arg);
                }

                if (arg.Equals("fpt", StringComparison.OrdinalIgnoreCase))
                {
                    this.ForceParametersTask = true;
                }

                if (arg.Equals("v", StringComparison.OrdinalIgnoreCase) || arg.Equals("verbose", StringComparison.OrdinalIgnoreCase))
                {
                    this.Verbose = true;
                }

                if (arg.Equals("pw", StringComparison.OrdinalIgnoreCase) || arg.Equals("preservewhitespace", StringComparison.OrdinalIgnoreCase))
                {
                    this.PreserveWhitespace = true;
                }

                if (arg.Equals("indent", StringComparison.OrdinalIgnoreCase))
                {
                    this.Indent = true;
                }
            }
        }

        private static string GetValueFromArguments(string arg)
        {
            int startIndex = arg.IndexOf(":", StringComparison.Ordinal) + 1;
            return arg.Substring(startIndex, arg.Length - startIndex).Trim('"');
        }
    }
}