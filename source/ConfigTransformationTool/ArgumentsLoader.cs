// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;
    using System.Text;

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

        public bool Quiet { get; private set; }

        public bool PreserveWhitespace { get; private set; }

        public bool Indent { get; private set; }

        public string IndentChars { get; private set; }

        public Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// Load arguments from command line
        /// </summary>
        /// <param name="args"></param>
        public bool Load(string[] args)
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
                    continue;
                }

                if (arg.IndexOf("t:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("transform:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.TransformFilePath = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("d:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("destination:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.DestinationFilePath = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("p:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("parameters:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.ParametersString = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("pf:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("parameters.file:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.ParametersFile = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.Equals("fpt", StringComparison.OrdinalIgnoreCase))
                {
                    this.ForceParametersTask = true;
                    continue;
                }

                if (arg.Equals("v", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("verbose", StringComparison.OrdinalIgnoreCase))
                {
                    this.Verbose = true;
                    continue;
                }

                if (arg.Equals("pw", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("preservewhitespace", StringComparison.OrdinalIgnoreCase))
                {
                    this.PreserveWhitespace = true;
                    continue;
                }

                if (arg.Equals("i", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("indent", StringComparison.OrdinalIgnoreCase))
                {
                    this.Indent = true;
                    continue;
                }

                if (arg.IndexOf("ic:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("indentchars:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.IndentChars = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("e:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("encoding:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var name = GetValueFromArguments(arg);

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.Error.WriteLine("encoding: or e: requires an parameter value.");
                        return false;
                    }

                    try
                    {
                        this.DefaultEncoding = GetEncoding(name);
                    }
                    catch
                    {
                        Console.Error.WriteLine("Cannot load encoding: {0}", name);
                        return false;
                    }
                }

                if (arg.Equals("q", StringComparison.OrdinalIgnoreCase)
                || arg.Equals("quiet", StringComparison.OrdinalIgnoreCase))
                {
                    this.Quiet = true;
                    continue;
                }
            }

            return true;
        }

        private static string GetValueFromArguments(string arg)
        {
            int startIndex = arg.IndexOf(":", StringComparison.Ordinal) + 1;
            string result = arg.Substring(startIndex, arg.Length - startIndex);

            // Do smart quotes trimming, only if we have quotes from both sides
            while (result.Length > 1 && result.IndexOf('"') == 0 && result.LastIndexOf('"') == (result.Length - 1))
            {
                result = result.Substring(1, result.Length - 2);
            }

            return result;
        }

        private static Encoding GetEncoding(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            // Encoding.GetEncoding is stupid. "UTF8" or "utf7" names cannot be used. Expects "UTF-8" or "utf-7"
            switch (name.ToLowerInvariant())
            {
                case "utf7": return Encoding.UTF7;
                case "utf8": return Encoding.UTF8;
                case "utf16": return Encoding.Unicode;
                case "utf32": return Encoding.UTF32;
            }

            return Encoding.GetEncoding(name);
        }
    }
}