using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ConfigTransformationTool.Base;
using log4net;
using log4net.Config;

namespace ConfigTransformationTool
{
	internal class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static int Main(string[] args)
		{
			try
			{
				ArgumentsLoader argumentsLoader = new ArgumentsLoader();

				XmlConfigurator.Configure();
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

				argumentsLoader.Load(args);

				if (argumentsLoader.IsAllRequiredParametersSet)
				{
					TransformationTask task = new TransformationTask(argumentsLoader.SourceFilePath, argumentsLoader.TransformFilePath);

					IDictionary<string, string> parameters = new Dictionary<string, string>();

					if (!string.IsNullOrWhiteSpace(argumentsLoader.ParametersString))
						ParametersParser.ReadParameters(argumentsLoader.ParametersString, parameters);

					if (!string.IsNullOrWhiteSpace(argumentsLoader.ParametersFile))
						ParametersLoader.LoadParameters(argumentsLoader.ParametersFile, parameters);

					task.SetParameters(parameters);

					if (!task.Execute(argumentsLoader.DestinationFilePath, argumentsLoader.ForceParametersTask))
						return 4;
				}
				else
				{
					ShowToolHelp();
					return 1;
				}

				return 0;
			} 
			catch(Exception e)
			{
				Log.Fatal(e);
				return 4;
			}
		}

		private static void ShowToolHelp()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string exeFile = assembly.ManifestModule.Name;

			Console.WriteLine("{0}, {1}, http://ctt.codeplex.com", GetTitleString(assembly), GetVersionString(assembly));
			Console.WriteLine("by OutcoldSolutions, http://outcoldman.com, {0}", DateTime.Today.Year);
			Console.WriteLine();
			Console.WriteLine("Arguments:");
			Console.WriteLine("  source:{file} (s:) - source file path");
			Console.WriteLine("  transform:{file} (t:) - transform file path");
			Console.WriteLine("  destination:{file} (d:) - destination file path");
			Console.WriteLine("  parameters:{parameters} (p:) - (Optional parameter) \r\n    string of parameters used expected by source file separated by ';',\r\n    value should be separated from name by ':',\r\n    if value contains spaces - quote it");
			Console.WriteLine("  parameters.file:{parameters file} (pf:) - (Optional parameter) \r\n    path to xml file which contains parameters with values \r\n    use xml schema ParametersSchema.xsd to make right file");
			Console.WriteLine("  fpt  - (Optional parameter) force parameters task \r\n    (if parameters argument is empty, but need to apply default values),\r\n    default is false");
			Console.WriteLine();
			Console.WriteLine("Examples:");
			Console.WriteLine();
			Console.WriteLine(string.Format("{0} source:\"source.config\"", exeFile));
			Console.WriteLine("\ttransform:\"transform.config\"");
			Console.WriteLine("\tdestination:\"destination.config\"");
			Console.WriteLine();
			Console.WriteLine(string.Format("{0} s:\"source.config\"", exeFile));
			Console.WriteLine("\tt:\"transform.config\"");
			Console.WriteLine("\td:\"destination.config\"");
			Console.WriteLine("\tp:Parameter1:\"Value of parameter1\";Parameter2:Value2");
			Console.WriteLine();
			Console.WriteLine("To get more details about transform file syntax go to");
			Console.WriteLine("http://msdn.microsoft.com/en-us/library/dd465326.aspx");
		}

		public static string GetTitleString(Assembly asm)
		{
			object[] attributes = asm.GetCustomAttributes(typeof(AssemblyTitleAttribute), true);
			if (attributes.Length > 0 && attributes[0] is AssemblyTitleAttribute)
				return (attributes[0] as AssemblyTitleAttribute).Title;

			return null;
		}

		public static string GetVersionString(Assembly asm)
		{
			if (asm != null && !string.IsNullOrEmpty(asm.FullName))
			{
				string[] parts = asm.FullName.Split(',');
				if (parts.Length > 1)
					return parts[1].Trim();
			}
			return null;
		}

		private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Log.Fatal(e.ExceptionObject);
		}
	}
}