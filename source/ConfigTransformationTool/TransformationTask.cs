// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;

    using Microsoft.Web.XmlTransform;

    /// <summary>
    /// Make transformation of file <see cref="SourceFilePath"/> with transform file <see cref="TransformFile"/>.
    /// Look at http://msdn.microsoft.com/en-us/library/dd465326.aspx for syntax of transformation file.
    /// </summary>
    public class TransformationTask
    {
        private readonly OutputLog _log;

        private readonly TransformationLogger _transformationLogger;

        private IDictionary<string, string> _parameters;

        private Encoding _defaultEncoding;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TransformationTask(OutputLog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _transformationLogger = new TransformationLogger(log);
            IndentChars = "    ";
        }

        /// <summary>
        /// Create new TransformationTask object and set values for <see cref="SourceFilePath"/> and <see cref="TransformFile"/>
        /// </summary>
        /// <param name="log">The logger.</param>
        /// <param name="sourceFilePath">Source file path</param>
        /// <param name="transformFilePath">Transformation file path</param>
        /// <param name="preserveWhitespace">Force to preserve all whitespaces in Xml Element and Xml Attributes values.</param>
        public TransformationTask(
            OutputLog log, 
            string sourceFilePath, 
            string transformFilePath,
            bool preserveWhitespace)
            : this(log)
        {
            SourceFilePath = sourceFilePath;
            TransformFile = transformFilePath;
            PreserveWhitespace = preserveWhitespace;
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
        /// Force to preserve all whitespaces in Xml Element and Xml Attributes values.
        /// </summary>
        public bool PreserveWhitespace { get; set; }

        /// <summary>
        /// Get or sets a value indicating wether the output Xml will be indented.
        /// </summary>
        public bool Indent { get; set; }

        /// <summary>
        /// Gets or sets the character string to use when indenting. 4 spaces is a default value.
        /// </summary>
        public string IndentChars { get; set; }

        /// <summary>
        /// Gets or sets the default encoding to use.
        /// </summary>
        public Encoding DefaultEncoding
        {
            get { return _defaultEncoding ?? Encoding.UTF8; }
            set { _defaultEncoding = value; }
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

            _log.WriteLine("Start tranformation to '{0}'.", destinationFilePath);

            if (string.IsNullOrWhiteSpace(SourceFilePath) || !File.Exists(SourceFilePath))
            {
                throw new FileNotFoundException("Can't find source file.", SourceFilePath);
            }

            if (string.IsNullOrWhiteSpace(TransformFile) || !File.Exists(TransformFile))
            {
                throw new FileNotFoundException("Can't find transform  file.", TransformFile);
            }

            _log.WriteLine("Source file: '{0}'.", SourceFilePath);
            _log.WriteLine("Transform  file: '{0}'.", TransformFile);

            try
            {
                Encoding encoding = DefaultEncoding;

                XmlDocument document = new XmlDocument()
                                           {
                                               PreserveWhitespace = PreserveWhitespace
                                           };

                document.Load(SourceFilePath);
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    var xmlDeclaration = (XmlDeclaration)document.FirstChild;
                    if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
                    {
                        encoding = Encoding.GetEncoding(xmlDeclaration.Encoding);
                    }
                }

                _log.WriteLine("Transformation task is using encoding '{0}'. Change encoding in source file, or use the 'encoding' parameter if you want to change encoding.", encoding);

                var transformFile = File.ReadAllText(TransformFile, encoding);

                if ((_parameters != null && _parameters.Count > 0) || forceParametersTask)
                {
                    ParametersTask parametersTask = new ParametersTask();
                    if (_parameters != null)
                    {
                        parametersTask.AddParameters(_parameters);
                    }

                    transformFile = parametersTask.ApplyParameters(transformFile);
                }

                XmlTransformation transformation = new XmlTransformation(transformFile, false, _transformationLogger);

                bool result = transformation.Apply(document);

                var outerXml = document.OuterXml;

                if (Indent)
                {
                    outerXml = GetIndentedOuterXml(outerXml, encoding);
                }

                if (PreserveWhitespace)
                {
                    outerXml = outerXml.Replace("&#xD;", "\r").Replace("&#xA;", "\n");
                }

                File.WriteAllText(destinationFilePath, outerXml, encoding);

                return result;
            }
            catch (Exception e)
            {
                _log.WriteErrorLine("Exception while transforming: {0}.", e);
                return false;
            }
        }

        private string GetIndentedOuterXml(string xml, Encoding encoding)
        {
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = IndentChars ?? new string(' ', 4),
                Encoding = encoding
            };

            using (var buffer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(buffer, xmlWriterSettings))
                {
                    XDocument.Parse(xml).WriteTo(xmlWriter);
                }

                return WorkAroundToRestoreProperXmlDeclarationTag(xml, buffer.ToString());
            }
        }

        private string WorkAroundToRestoreProperXmlDeclarationTag(string xml, string indentedXml)
        {
            var xmlRegex = new Regex(@"^(<\?xml.*\?>\s*)", RegexOptions.Singleline);
            var match = xmlRegex.Match(xml);
            return !match.Success
                ? xmlRegex.Replace(indentedXml, string.Empty)
                : xmlRegex.Replace(indentedXml, match.Groups[1].Value);
        }
    }
}