using System;
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
			ArgumentsLoader argumentsLoader = new ArgumentsLoader();

			XmlConfigurator.Configure();
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

			argumentsLoader.Load(args);

			if (argumentsLoader.IsAllParametersSet)
			{
				TransformationTask task = new TransformationTask(argumentsLoader.SourceFilePath, argumentsLoader.TransformFilePath);
				if (task.Execute(argumentsLoader.DestinationFilePath))
					return 4;
			}
			else
			{
				ShowToolHelp();
				return 1;
			}

			return 0;
		}

		private static void ShowToolHelp()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string exeFile = assembly.ManifestModule.Name;

			Console.WriteLine(string.Format("{0}, {1}, http://ctt.codeplex.com", GetTitleString(assembly), GetVersionString(assembly)));
			Console.WriteLine("by outcoldman, http://outcoldman.ru, 2010");
			Console.WriteLine();
			Console.WriteLine("Arguments:");
			Console.WriteLine("\tsource: (s:) - source file path");
			Console.WriteLine("\ttransform: (t:) - transform file path");
			Console.WriteLine("\tdestination: (d:) - destination file path");
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