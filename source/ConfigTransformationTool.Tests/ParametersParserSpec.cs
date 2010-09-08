using ConfigTransformationTool.Base;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace ConfigTransformationTool.Tests
{
	[TestFixture]
	class ParametersParserSpec
	{
		/// <summary>
		/// Check simple parameters command line
		/// </summary>
		[Test]
		public void Sample()
		{
			const string parametersLine = "Parameter1:Value1;Parameter2:121.232";

			var parameters = ParametersParser.ReadParameters(parametersLine);

			Assert.AreEqual("Value1", parameters["Parameter1"]);
			Assert.AreEqual("121.232", parameters["Parameter2"]);
		}

		/// <summary>
		/// Check parameters command line when one of parameter has semi in value string
		/// </summary>
		[Test]
		public void String_With_Semicolon_In_Value()
		{
			const string parametersLine = "Parameter1:Value1;Parameter2:\"121;232\"";

			var parameters = ParametersParser.ReadParameters(parametersLine);

			Assert.AreEqual("Value1", parameters["Parameter1"]);
			Assert.AreEqual("121;232", parameters["Parameter2"]);
		}

		/// <summary>
		/// Check that if command line has semicon at end parameters will be loaded
		/// </summary>
		[Test]
		public void String_With_Semicolon_At_End()
		{
			const string parametersLine = "Parameter1:Value1;Parameter2:\"121.232\";";

			var parameters = ParametersParser.ReadParameters(parametersLine);

			Assert.AreEqual("Value1", parameters["Parameter1"]);
			Assert.AreEqual("121.232", parameters["Parameter2"]);
		}

		/// <summary>
		/// Check that value of parameter can contain escaped quotes
		/// </summary>
		[Test]
		public void String_With_Values_With_Quotes()
		{
			const string parametersLine = @"Parameter1:Value1;Parameter2:""12\""1.2\""32"";";

			var parameters = ParametersParser.ReadParameters(parametersLine);

			Assert.AreEqual("Value1", parameters["Parameter1"]);
			Assert.AreEqual("12\"1.2\"32", parameters["Parameter2"]);
		}
	}
}
