// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    using System;
    using System.IO;
    using System.Text;

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

            string fileContent = File.ReadAllText(resultFile);
            
            // Check that transformation happend
            Assert.IsTrue(fileContent.Contains(@"value=""601"""));
        }

        [Test]
        public void TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines()
        {
            const string Source = @"<?xml version=""1.0""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWillBeReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"" xdt:Transform=""Replace""  xdt:Locator=""Match(name)"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Expected = @"<?xml version=""1.0""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepNewLines_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            var sut = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
                {
                    Indent = true
                };

            // Act
            var actualResult = sut.Execute(resultFile);
            var actualContents = File.ReadAllText(resultFile);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(Expected, actualContents);
        }

        [Test]
        public void TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration()
        {
            const string Source = @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWillBeReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"" xdt:Transform=""Replace""  xdt:Locator=""Match(name)"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            const string Expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<configuration>
    <configSections>
        <sectionGroup name=""userSettings"" type=""System.Configuration.UserSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"">
            <section name=""MyNamespace.Properties.Settings"" type=""System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"" allowExeDefinition=""MachineToLocalUser"" requirePermission=""false"" />
        </sectionGroup>
    </configSections>
    <userSettings>
        <MyNamespace.Properties.Settings>
            <setting name=""MyConfiguration"" serializeAs=""String"">
                <value>ThisWasReplaced</value>
            </setting>
        </MyNamespace.Properties.Settings>
    </userSettings>
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithNewLineBetweenTags_ShouldKeepXmlDeclaration_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source);

            // Create transform file
            this.WriteToFile(transformFile, Transform);

            var sut = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true
            };

            // Act
            var actualResult = sut.Execute(resultFile);
            var actualContents = File.ReadAllText(resultFile);

            // Assert
            Assert.IsTrue(actualResult);
            Assert.AreEqual(Expected, actualContents);
        }

        [Test]
        public void TransfarmotaionWithNewLineInValue_ShouldKeepNewLine()
        {
            const string Source = @"<?xml version=""1.0""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >

	<value key=""Test1"" value=""Tru
e"" />

</configuration>";

            const string Transform = @"<?xml version=""1.0""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	
	<value key=""Test1"" value=""60
1&lt;group name=&quot;&quot;"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
	
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

            string fileContent = File.ReadAllText(resultFile);

            Assert.IsTrue(fileContent.Contains(string.Format(@"value=""60{0}1&lt;group name=&quot;&quot;""", Environment.NewLine)));
        }

        [Test]
        public void TransfarmotaionWithChineseCharaters_ShouldTransform()
        {
            const string Source = @"<?xml version=""1.0"" encoding=""utf-8""?>

<configuration xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"" >
	<custom key=""Test"" value="""" />
</configuration>";

            const string Transform = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"" xmlns=""http://schemas.itisnotadomain/Configuration/v2.0"">
	<custom key=""Test"" value=""倉頡; 仓颉"" xdt:Transform=""Replace""  xdt:Locator=""Match(key)"" />
</configuration>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "TransfarmotaionWithChineseCharaters_ShouldTransform.config");
            string transformFile = Path.Combine(baseDirectory, "TransfarmotaionWithChineseCharaters_ShouldTransform_transform.config");
            string resultFile = Path.Combine(baseDirectory, "TransfarmotaionWithChineseCharaters_ShouldTransform_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true);
            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);

            Assert.IsTrue(fileContent.Contains(@"value=""倉頡; 仓颉"""));
        }

        [Test]
        public void Issue442278()
        {
            const string Source = @"<?xml version=""1.0""?>
<connectionStrings>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
</connectionStrings>";

            const string Transform = @"<?xml version=""1.0""?>
<connectionStrings xdt:Transform=""Replace"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
        <a>aaa</a>
        <b>bbb</b>
        <c>ccc</c>
</connectionStrings>";

            const string Result = @"<?xml version=""1.0""?>
<connectionStrings>
    <a>aaa</a>
    <b>bbb</b>
    <c>ccc</c>
</connectionStrings>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Issue442278.config");
            string transformFile = Path.Combine(baseDirectory, "Issue442278_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Issue442278_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
                                          {
                                              Indent = true
                                          };

            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);

            Assert.AreEqual(Result, fileContent);
        }

        [Test]
        public void Issue442278_CanSpecifyIndentChars()
        {
            const string Source = @"<?xml version=""1.0""?>
<connectionStrings>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
        <x></x>
</connectionStrings>";

            const string Transform = @"<?xml version=""1.0""?>
<connectionStrings xdt:Transform=""Replace"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
        <a>aaa</a>
        <b>bbb</b>
        <c>ccc</c>
</connectionStrings>";

            const string Result = @"<?xml version=""1.0""?>
<connectionStrings>
  <a>aaa</a>
  <b>bbb</b>
  <c>ccc</c>
</connectionStrings>";

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(baseDirectory, "Issue442278.config");
            string transformFile = Path.Combine(baseDirectory, "Issue442278_transform.config");
            string resultFile = Path.Combine(baseDirectory, "Issue442278_result.config");

            // Create source file
            this.WriteToFile(sourceFile, Source, Encoding.UTF8);

            // Create transform file
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            TransformationTask task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true,
                IndentChars = "  "
            };

            Assert.IsTrue(task.Execute(resultFile));

            string fileContent = File.ReadAllText(resultFile, Encoding.UTF8);

            Assert.AreEqual(Result, fileContent);
        }

        [Test]
        public void Execute_IndentTrueIndentCharsNull_DoesNotThrowException()
        {
            const string Source = @"<?xml version=""1.0""?>
<connectionStrings>
        <x></x>
</connectionStrings>";

            const string Transform = @"<?xml version=""1.0""?>
<connectionStrings xdt:Transform=""Replace"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
        <a>aaa</a>
</connectionStrings>";

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var sourceFile = Path.Combine(baseDirectory, "Execute_IndentTrueIndentCharsNull_DoesNotThrowException-Source.config");
            var transformFile = Path.Combine(baseDirectory, "Execute_IndentTrueIndentCharsNull_DoesNotThrowException-Transform.config");
            var resultFile = Path.Combine(baseDirectory, "Execute_IndentTrueIndentCharsNull_DoesNotThrowException-Result.config");

            this.WriteToFile(sourceFile, Source, Encoding.UTF8);
            this.WriteToFile(transformFile, Transform, Encoding.UTF8);

            var task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                Indent = true,
                IndentChars = null
            };

            Assert.IsTrue(task.Execute(resultFile));
        }

        [Test]
        public void Execute_TransformFileDoesNotContainUtf8ByteOrderMark_EncodingSpecifiedToUtf8__DoesNotThrowException()
        {
            const string Source = @"<connectionStrings>
        <x></x>
</connectionStrings>";

            const string Transform = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<connectionStrings xdt:Transform=""Replace"" xmlns:xdt=""http://schemas.microsoft.com/XML-Document-Transform"">
        <a>aaa</a>
</connectionStrings>";

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var sourceFile = Path.Combine(baseDirectory, "Execute_IndentTrueIndentCharsNull_DoesNotThrowException-Source.config");
            var transformFile = Path.Combine(baseDirectory, "Execute_IndentTrueIndentCharsNull_DoesNotThrowException-Transform.config");
            var resultFile = Path.Combine(baseDirectory, "Execute_IndentTrueIndentCharsNull_DoesNotThrowException-Result.config");

            this.WriteToFile(sourceFile, Source, Encoding.UTF8);
            this.WriteToFile(transformFile, Transform, Encoding.ASCII);

            var task = new TransformationTask(this.Log, sourceFile, transformFile, preserveWhitespace: true)
            {
                // This is the equivilent as specifying "encoding:UTF8"
                DefaultEncoding = Encoding.UTF8
            };

            Assert.IsTrue(task.Execute(resultFile));
        }
    }
}
