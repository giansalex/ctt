using System;
using System.IO;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using NUnit.Framework;

namespace ConfigTransformationTool.Tests
{
	/// <summary>
	/// Base test class. Configure log4net in <see cref="SetUp"/>.
	/// </summary>
	public abstract class BaseSpec
	{
		[SetUp]
		public void SetUp()
		{
			ConfigureLogger();
		}

		/// <summary>
		/// Write <paramref name="content"/> to <paramref name="filePath"/>. If file <paramref name="filePath"/> exists it will be overwritten.
		/// </summary>
		/// <param name="filePath">File path</param>
		/// <param name="content">Content of file</param>
		protected void WriteToFile(string filePath, string content)
		{
			File.WriteAllText(filePath, content);
		}

		/// <summary>
		/// Configure log4net for showing trace information in output window.
		/// </summary>
		private static void ConfigureLogger()
		{
			PatternLayout patternLayout = new PatternLayout
			                              	{
			                              		ConversionPattern =
			                              			"%message%newline"
			                              	};
			patternLayout.ActivateOptions();

			ForwardingAppender appender = new ForwardingAppender();

			TraceAppender traceAppender = new TraceAppender {Layout = patternLayout};
			appender.AddAppender(traceAppender);

			BasicConfigurator.Configure(appender);
		}
	}
}