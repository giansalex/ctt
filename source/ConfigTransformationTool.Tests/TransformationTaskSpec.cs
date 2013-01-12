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
        private const string Source = @"<?xml version=""1.0""?>

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

        private const string Transform = @"<?xml version=""1.0""?>
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

        /// <summary>
        /// Simple test 
        /// </summary>
        [Test]
        public void Transfarmotaion_Should_Happend()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend.config");
            string transformFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Transfarmotaion_Should_Happend_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent;

            using (StreamReader reader = new StreamReader(resultFile))
            {
                fileContent = reader.ReadToEnd();
            }

            // Check that transformation happend
            Assert.IsTrue(fileContent.Contains(@"value=""601"""));
        }
    }
}
