// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    using System;
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    /// <summary>
    /// Base test class. Configure log4net in <see cref="SetUp"/>.
    /// </summary>
    public abstract class BaseSpec
    {
        protected OutputLog Log { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            this.Log = OutputLog.FromWriter(Console.Out);
        }

        /// <summary>
        /// Write <paramref name="content"/> to <paramref name="filePath"/>. If file <paramref name="filePath"/> exists it will be overwritten.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="content">Content of file</param>
        protected void WriteToFile(string filePath, string content)
        {
            WriteToFile(filePath, content, Encoding.Unicode);
        }

        /// <summary>
        /// Write <paramref name="content"/> to <paramref name="filePath"/>. If file <paramref name="filePath"/> exists it will be overwritten.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="content">Content of file</param>
        /// <param name="encoding">The encoding.</param>
        protected void WriteToFile(string filePath, string content, Encoding encoding)
        {
            File.WriteAllText(filePath, content, encoding);
        }
    }
}