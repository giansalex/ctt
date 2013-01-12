// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace ConfigTransformationTool.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using ConfigTransformationTool.Base;

    using NUnit.Framework;

    [TestFixture]
    public class ParametersLoaderSpec : BaseSpec
    {
        private const string ParametersFileContent = @"<?xml version=""1.0""?>
<parameters>
<param name=""ServerName"" value=""disneyland"" />
<param name=""RootDirectory"" value=""D:\Disneyland"" />
<param name=""DefaultUser"" value=""MickeyMouse"" />
</parameters>";

        [Test]
        public void LoadParameters_FileWithParametes_ParametersShouldBeLoaded()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string parametersFile = Path.Combine(baseDirectory, "ParametersFile.xml");
            this.WriteToFile(parametersFile, ParametersFileContent);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            ParametersLoader.LoadParameters(parametersFile, parameters);

            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("disneyland", parameters["ServerName"]);
            Assert.AreEqual("D:\\Disneyland", parameters["RootDirectory"]);
            Assert.AreEqual("MickeyMouse", parameters["DefaultUser"]);
        }
    }
}