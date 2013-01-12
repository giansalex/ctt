// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace ConfigTransformationTool.Base
{
    using System;
    using System.Reflection;

    using log4net;

    using Microsoft.Web.XmlTransform;

    // Simple implementation of logger
    public class TransformationLogger : IXmlTransformationLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void LogMessage(string message, params object[] messageArgs)
        {
            Log.InfoFormat(message, messageArgs);
        }

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            Log.InfoFormat(message, messageArgs);
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            Log.WarnFormat(message, messageArgs);
        }

        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            Log.WarnFormat(string.Format("File: {0}, Message: {1}", file, message), messageArgs);
        }

        public void LogWarning(
            string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            Log.WarnFormat(
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
            Log.ErrorFormat(message, messageArgs);
        }

        public void LogError(string file, string message, params object[] messageArgs)
        {
            Log.ErrorFormat(string.Format("File: {0}, Message: {1}", file, message), messageArgs);
        }

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            Log.ErrorFormat(
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
            Log.Error(ex);
        }

        public void LogErrorFromException(Exception ex, string file)
        {
            Log.Error(file, ex);
        }

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            Log.Error(
                string.Format("File: {0}, LineNumber: {1}, LinePosition: {2}", file, lineNumber, linePosition), ex);
        }

        public void StartSection(string message, params object[] messageArgs)
        {
            Log.InfoFormat(message, messageArgs);
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            Log.InfoFormat(message, messageArgs);
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            Log.InfoFormat(message, messageArgs);
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            Log.InfoFormat(message, messageArgs);
        }
    }
}