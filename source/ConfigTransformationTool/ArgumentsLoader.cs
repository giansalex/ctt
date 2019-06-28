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
                return !string.IsNullOrWhiteSpace(SourceFilePath)
                    && !string.IsNullOrWhiteSpace(TransformFilePath)
                    && !string.IsNullOrWhiteSpace(DestinationFilePath);
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
            DestinationFilePath = string.Empty;
            SourceFilePath = string.Empty;
            TransformFilePath = string.Empty;
            ParametersString = string.Empty;
            ParametersFile = string.Empty;
            ForceParametersTask = false;
            Verbose = false;
            PreserveWhitespace = false;
            Indent = false;

            foreach (string arg in args)
            {
                if (arg.IndexOf("s:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("source:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    SourceFilePath = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("t:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("transform:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    TransformFilePath = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("d:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("destination:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    DestinationFilePath = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("p:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("parameters:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ParametersString = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.IndexOf("pf:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("parameters.file:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    ParametersFile = GetValueFromArguments(arg);
                    continue;
                }

                if (arg.Equals("fpt", StringComparison.OrdinalIgnoreCase))
                {
                    ForceParametersTask = true;
                    continue;
                }

                if (arg.Equals("v", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("verbose", StringComparison.OrdinalIgnoreCase))
                {
                    Verbose = true;
                    continue;
                }

                if (arg.Equals("pw", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("preservewhitespace", StringComparison.OrdinalIgnoreCase))
                {
                    PreserveWhitespace = true;
                    continue;
                }

                if (arg.Equals("i", StringComparison.OrdinalIgnoreCase)
                    || arg.Equals("indent", StringComparison.OrdinalIgnoreCase))
                {
                    Indent = true;
                    continue;
                }

                if (arg.IndexOf("ic:", StringComparison.OrdinalIgnoreCase) == 0
                    || arg.IndexOf("indentchars:", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    IndentChars = GetValueFromArguments(arg);
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
                        DefaultEncoding = GetEncoding(name);
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
                    Quiet = true;
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