// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    using System;
    using System.IO;

    using NUnit.Framework;

    [TestFixture]
    public class TransformationTaskSpec : BaseSpec
    {
        [Test]
        public void Transfarmotaion_Should_Happend()
        {
            const string Source = @"<?xml version=""1.0""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >

	<custom>
		<groups>
			<group name=""TestGroup1"">
				<values>
					<value key=""Test1"" value=""True"" />
					<value key=""Test2"" value=""600"" />
				</values>
			</group>

			<group name=""TestGroup2"">
				<values>
					<value key=""Test3"" value=""True"" />
				</values>
			</group>

		</groups>
	</custom>
	
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	
	<custom>
		<groups>
			<group name=""TestGroup1"">
				<values>
					<value key=""Test2"" value=""601"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
				</values>
			</group>
		</groups>
	</custom>
	
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend.config");
            string transformFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: false);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent;

            using (StreamReader reader = new StreamReader(resultFile))
            {
                fileContent = reader.ReadToEnd();
            }

            // Check that transformation happend
            Assert.IsTrue(fileContent.Contains(@"value=""601"""));
        }

        [Test]
        public void TransfarmotaionWithNewLineInValue_ShouldKeepNewLine()
        {
            const string Source = @"<?xml version=""1.0""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >

	<custom>
		<groups>
			<group name=""TestGroup1"">
				<values>
					<value key=""Test1"" value=""Tru
e"" />
				</values>
			</group>
		</groups>
	</custom>
	
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	
	<custom>
		<groups>
			<group name=""TestGroup1"">
				<values>
					<value key=""Test1"" value=""60
1&lt;group name=&quot;&quot;"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
				</values>
			</group>
		</groups>
	</custom>
	
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineInValue_ShouldKeepNewLine.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineInValue_ShouldKeepNewLine_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineInValue_ShouldKeepNewLine_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent;

            using (StreamReader reader = new StreamReader(resultFile))
            {
                fileContent = reader.ReadToEnd();
            }

            Assert.IsTrue(fileContent.Contains(string.Format(@"value=""60{0}1&lt;group name=&quot;&quot;""", Environment.NewLine)));
        }
    }
}
