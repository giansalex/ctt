// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;
    using System.IO;

    public class OutputLog
    {
        private readonly TextWriter outputStream;

        private OutputLog(TextWriter outputStream)
        {
            this.outputStream = outputStream;
        }

        private OutputLog()
        {
        }

        public static OutputLog Empty()
        {
            return new OutputLog();
        }

        public static OutputLog FromWriter(TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            return new OutputLog(writer);
        }

        public void WriteLine(string message, params object[] parameters)
        {
            if (this.outputStream != null)
            {
                this.outputStream.WriteLine(message, parameters);
            }
        }
    }
}