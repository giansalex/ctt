using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net;
using Microsoft.Web.Publishing.Tasks;

namespace ConfigTransformationTool.Base
{
	/// <summary>
	/// Make transformation of file <see cref="SourceFilePath"/> with transform file <see cref="TransformFile"/>.
	/// Look at http://msdn.microsoft.com/en-us/library/dd465326.aspx for syntax of transformation file.
	/// </summary>
	public class TransformationTask
	{
		private readonly static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType); 

		private readonly TransformationLogger _transfomrationLogger = new TransformationLogger();

		private IDictionary<string, string> _parameters;

		/// <summary>
		/// Empty constructor
		/// </summary>
		public TransformationTask()
		{
		}

		/// <summary>
		/// Create new TransformationTask object and set values for <see cref="SourceFilePath"/> and <see cref="TransformFile"/>
		/// </summary>
		/// <param name="sourceFilePath">Source file path</param>
		/// <param name="transformFilePath">Transformation file path</param>
		public TransformationTask(string sourceFilePath, string transformFilePath)
		{
			Log.Debug("Create Transformation Task.");

			SourceFilePath = sourceFilePath;
			TransformFile = transformFilePath;
		}

		/// <summary>
		/// Set parameters and values for transform
		/// </summary>
		/// <param name="parameters">Dictionary of parameters with values.</param>
		public void SetParameters(IDictionary<string, string> parameters)
		{
			_parameters = parameters;
		}

		/// <summary>
		/// Source file
		/// </summary>
		public string SourceFilePath { get; set; }

		/// <summary>
		/// Transformation file
		/// </summary>
		/// <remarks>
		/// See http://msdn.microsoft.com/en-us/library/dd465326.aspx for syntax of transformation file
		/// </remarks>
		public string TransformFile { get; set; }

		/// <summary>
		/// Make transformation of file <see cref="SourceFilePath"/> with transform file <see cref="TransformFile"/> to <paramref name="destinationFilePath"/>.
		/// </summary>
		/// <param name="destinationFilePath">File path of destination transformation.</param>
		/// <param name="forceParametersTask">Invoke parameters task even if the parameters are not set with <see cref="SetParameters" />.</param>
		/// <returns>Return true if transformation finish successfully, otherwise false.</returns>
		public bool Execute(string destinationFilePath, bool forceParametersTask = false)
		{
			if (string.IsNullOrWhiteSpace(destinationFilePath))
				throw new ArgumentException("Destination file can't be empty.", "destinationFilePath");

			Log.DebugFormat("Start tranformation to '{0}'.", destinationFilePath);

			if (string.IsNullOrWhiteSpace(SourceFilePath) || !File.Exists(SourceFilePath))
				throw new FileNotFoundException("Can't find source file.", SourceFilePath);

			if (string.IsNullOrWhiteSpace(TransformFile) || !File.Exists(TransformFile))
				throw new FileNotFoundException("Can't find transform  file.", TransformFile);

			Log.DebugFormat("Source file: '{0}'.", SourceFilePath);
			Log.DebugFormat("Transform  file: '{0}'.", TransformFile);

			try
			{
				var transformFile = ReadContent(TransformFile);

				if (_parameters != null || forceParametersTask)
				{
					ParametersTask parametersTask = new ParametersTask();
					if (_parameters != null)
						parametersTask.AddParameters(_parameters);
					transformFile = parametersTask.ApplyParameters(transformFile);
				}

				XmlDocument document = new XmlDocument();
				document.Load(SourceFilePath);
				
				XmlTransformation transformation = new XmlTransformation(transformFile, false, _transfomrationLogger);
				
				bool result = transformation.Apply(document);

				document.Save(destinationFilePath);

				return result;
			} 
			catch(Exception e)
			{
				Log.Error(e);
				return false;
			}
		}

		private static string ReadContent(string path)
		{
			using (StreamReader reader = new StreamReader(path))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
