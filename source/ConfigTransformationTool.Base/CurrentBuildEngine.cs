using System;
using System.Collections;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Build.Framework;

namespace ConfigTransformationTool.Base
{
	/// <summary>
	/// Current build engine. Use log4net for logging.
	/// </summary>
	internal class CurrentBuildEngine : IBuildEngine
	{
		public readonly static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); 

		public void LogErrorEvent(BuildErrorEventArgs e)
		{
			StringBuilder logMessage = new StringBuilder();

			logMessage.AppendLine(e.Message);
			logMessage.AppendFormat("\tFile: {0}", e.File);
			logMessage.AppendFormat("\tCode: {0}", e.Code);
			logMessage.AppendFormat("\tColumnNumber: {0}", e.ColumnNumber);
			logMessage.AppendFormat("\tEndColumnNumber: {0}", e.EndColumnNumber);
			logMessage.AppendFormat("\tLineNumber: {0}", e.LineNumber);
			logMessage.AppendFormat("\tEndLineNumber: {0}", e.EndLineNumber);

			Log.Error(e.Message);
		}

		public void LogWarningEvent(BuildWarningEventArgs e)
		{
			StringBuilder logMessage = new StringBuilder();

			logMessage.AppendLine(e.Message);
			logMessage.AppendFormat("\tFile: {0}", e.File);
			logMessage.AppendFormat("\tCode: {0}", e.Code);
			logMessage.AppendFormat("\tColumnNumber: {0}", e.ColumnNumber);
			logMessage.AppendFormat("\tEndColumnNumber: {0}", e.EndColumnNumber);
			logMessage.AppendFormat("\tLineNumber: {0}", e.LineNumber);
			logMessage.AppendFormat("\tEndLineNumber: {0}", e.EndLineNumber);

			Log.Warn(logMessage);
		}

		public void LogMessageEvent(BuildMessageEventArgs e)
		{
			Log.Debug(e.Message);
		}

		public void LogCustomEvent(CustomBuildEventArgs e)
		{
			Log.Info(e.Message);
		}

		public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Foo implementation
		/// </summary>
		public bool ContinueOnError
		{
			get { return false; }
		}

		/// <summary>
		/// Foo implementation
		/// </summary>
		public int LineNumberOfTaskNode
		{
			get { return 0; }
		}

		/// <summary>
		/// Foo implementation
		/// </summary>
		public int ColumnNumberOfTaskNode
		{
			get { return 0;  }
		}

		/// <summary>
		/// Foo implementation
		/// </summary>
		public string ProjectFileOfTaskNode
		{
			get { return "Foo";  }
		}
	}
}
