// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    using System.Collections.Generic;

    using NUnit.Framework;

    [TestFixture]
    public class ParametersParserSpec : BaseSpec
    {
        private ParametersParser parser;

        public override void SetUp()
        {
            base.SetUp();

            this.parser = new ParametersParser(this.Log);
        }

        /// <summary>
        /// Check simple parameters command line
        /// </summary>
        [Test]
        public void Sample()
        {
            const string ParametersLine = "Parameter1:Value1;Parameter2:121.232";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            this.parser.ReadParameters(ParametersLine, parameters);

            Assert.AreEqual("Value1", parameters["Parameter1"]);
            Assert.AreEqual("121.232", parameters["Parameter2"]);
        }

        /// <summary>
        /// Check parameters command line when one of parameter has semi in value string
        /// </summary>
        [Test]
        public void String_With_Semicolon_In_Value()
        {
            const string ParametersLine = "Parameter1:Value1;Parameter2:\"121;232\"";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            this.parser.ReadParameters(ParametersLine, parameters);

            Assert.AreEqual("Value1", parameters["Parameter1"]);
            Assert.AreEqual("121;232", parameters["Parameter2"]);
        }

        /// <summary>
        /// Check that if command line has semicon at end parameters will be loaded
        /// </summary>
        [Test]
        public void String_With_Semicolon_At_End()
        {
            const string ParametersLine = "Parameter1:Value1;Parameter2:\"121.232\";";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            this.parser.ReadParameters(ParametersLine, parameters);

            Assert.AreEqual("Value1", parameters["Parameter1"]);
            Assert.AreEqual("121.232", parameters["Parameter2"]);
        }

        /// <summary>
        /// Check that value of parameter can contain escaped quotes
        /// </summary>
        [Test]
        public void String_With_Values_With_Quotes()
        {
            const string ParametersLine = @"Parameter1:Value1;Parameter2:""12\""1.2\""32"";";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            this.parser.ReadParameters(ParametersLine, parameters);

            Assert.AreEqual("Value1", parameters["Parameter1"]);
            Assert.AreEqual("12\"1.2\"32", parameters["Parameter2"]);
        }

        /// <summary>
        /// Verify that parameters values with ampersands are valid.
        /// </summary>
        [Test]
        public void ReadParameters_InputStringContainsAmpersand()
        {
            const string ParametersLine = @"Parameter1:Value1;Parameter2:""p$amp;wd"";";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            this.parser.ReadParameters(ParametersLine, parameters);

            Assert.AreEqual("Value1", parameters["Parameter1"]);
            Assert.AreEqual("p$amp;wd", parameters["Parameter2"]);
        }
    }
}
