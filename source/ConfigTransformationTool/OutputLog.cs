// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;
    using System.IO;

    public class OutputLog
    {
        private readonly TextWriter _outputStream;
        private readonly TextWriter _errorStream;

        private OutputLog(TextWriter outputStream, TextWriter errorStream)
        {
            _outputStream = outputStream;
            _errorStream = errorStream;
        }

        public static OutputLog ErrorOnly(TextWriter errorStream)
        {
            if (errorStream == null)
            {
                throw new ArgumentNullException(nameof(errorStream));
            }

            return new OutputLog(null, errorStream);
        }

        public static OutputLog FromWriter(TextWriter writer, TextWriter errorStream)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (errorStream == null)
            {
                throw new ArgumentNullException(nameof(errorStream));
            }

            return new OutputLog(writer, errorStream);
        }

        public static OutputLog NoLogs()
        {
            return new OutputLog(null, null);
        }
        public void WriteLine(string message, params object[] parameters)
        {
            _outputStream?.WriteLine(message, parameters);
        }

        public void WriteErrorLine(string message, params object[] parameters)
        {
            _errorStream?.WriteLine(message, parameters);
        }
    }
}