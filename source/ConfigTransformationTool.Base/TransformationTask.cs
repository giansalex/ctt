// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace ConfigTransformationTool.Base
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    using log4net;

    using Microsoft.Web.XmlTransform;

    /// <summary>
    /// Make transformation of file <see cref="SourceFilePath"/> with transform file <see cref="TransformFile"/>.
    /// Look at http://msdn.microsoft.com/en-us/library/dd465326.aspx for syntax of transformation file.
    /// </summary>
    public class TransformationTask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TransformationLogger transfomrationLogger = new TransformationLogger();

        private IDictionary<string, string> parameters;

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

            this.SourceFilePath = sourceFilePath;
            this.TransformFile = transformFilePath;
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
        /// Set parameters and values for transform
        /// </summary>
        /// <param name="parameters">Dictionary of parameters with values.</param>
        public void SetParameters(IDictionary<string, string> parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// Make transformation of file <see cref="SourceFilePath"/> with transform file <see cref="TransformFile"/> to <paramref name="destinationFilePath"/>.
        /// </summary>
        /// <param name="destinationFilePath">File path of destination transformation.</param>
        /// <param name="forceParametersTask">Invoke parameters task even if the parameters are not set with <see cref="SetParameters" />.</param>
        /// <returns>Return true if transformation finish successfully, otherwise false.</returns>
        public bool Execute(string destinationFilePath, bool forceParametersTask = false)
        {
            if (string.IsNullOrWhiteSpace(destinationFilePath))
            {
                throw new ArgumentException("Destination file can't be empty.", "destinationFilePath");
            }

            Log.DebugFormat("Start tranformation to '{0}'.", destinationFilePath);

            if (string.IsNullOrWhiteSpace(this.SourceFilePath) || !File.Exists(this.SourceFilePath))
            {
                throw new FileNotFoundException("Can't find source file.", this.SourceFilePath);
            }

            if (string.IsNullOrWhiteSpace(this.TransformFile) || !File.Exists(this.TransformFile))
            {
                throw new FileNotFoundException("Can't find transform  file.", this.TransformFile);
            }

            Log.DebugFormat("Source file: '{0}'.", this.SourceFilePath);
            Log.DebugFormat("Transform  file: '{0}'.", this.TransformFile);

            try
            {
                var transformFile = ReadContent(this.TransformFile);

                if ((this.parameters != null && this.parameters.Count > 0) || forceParametersTask)
                {
                    ParametersTask parametersTask = new ParametersTask();
                    if (this.parameters != null)
                    {
                        parametersTask.AddParameters(this.parameters);
                    }

                    transformFile = parametersTask.ApplyParameters(transformFile);
                }

                XmlDocument document = new XmlDocument();
                document.Load(this.SourceFilePath);

                XmlTransformation transformation = new XmlTransformation(transformFile, false, this.transfomrationLogger);

                bool result = transformation.Apply(document);

                document.Save(destinationFilePath);

                return result;
            }
            catch (Exception e)
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