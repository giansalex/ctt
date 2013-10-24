// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;

    using Microsoft.Web.XmlTransform;

    // Simple implementation of logger
    public class TransformationLogger : IXmlTransformationLogger
    {
        private readonly OutputLog log;

        public TransformationLogger(OutputLog log)
        {
            this.log = log;
        }

        public void LogMessage(string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }

        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            this.log.WriteLine(string.Format("File: {0}, Message: {1}", file, message), messageArgs);
        }

        public void LogWarning(
            string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            this.log.WriteLine(
                string.Format(
                    "File: {0}, LineNumber: {1}, LinePosition: {2}, Message: {3}",
                    file,
                    lineNumber,
                    linePosition,
                    message),
                messageArgs);
        }

        public void LogError(string message, params object[] messageArgs)
        {
            this.log.WriteErrorLine(message, messageArgs);
        }

        public void LogError(string file, string message, params object[] messageArgs)
        {
            this.log.WriteErrorLine(string.Format("File: {0}, Message: {1}", file, message), messageArgs);
        }

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            this.log.WriteErrorLine(
                string.Format(
                    "File: {0}, LineNumber: {1}, LinePosition: {2}, Message: {3}",
                    file,
                    lineNumber,
                    linePosition,
                    message),
                messageArgs);
        }

        public void LogErrorFromException(Exception ex)
        {
            this.log.WriteErrorLine("{0}", ex);
        }

        public void LogErrorFromException(Exception ex, string file)
        {
            this.log.WriteErrorLine("Exception {0} while reading {1}.", ex, file);
        }

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            this.log.WriteErrorLine("Exception {0} while reading {1}: LineNumber: {2}, LinePosition: {3}", ex, file, lineNumber, linePosition);
        }

        public void StartSection(string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            this.log.WriteLine(message, messageArgs);
        }
    }
}