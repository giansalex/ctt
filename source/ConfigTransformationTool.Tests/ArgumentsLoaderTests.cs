namespace OutcoldSolutions.ConfigTransformationTool.Suites
{
    using System.Collections;
    using System.Text;
    using NUnit.Framework;

    [TestFixture]
    public class ArgumentsLoaderTests
    {
        public IEnumerable Load_Encoding_SetsDefaultEncoding_TestCases
        {
            get
            {
                yield return new TestCaseData(null, null).Returns(true);
                yield return new TestCaseData("", null).Returns(true);
                yield return new TestCaseData("encoding:utf-8", Encoding.UTF8).Returns(true);
                yield return new TestCaseData("encoding:utf8", Encoding.UTF8).Returns(true);
                yield return new TestCaseData("encoding:UtF8", Encoding.UTF8).Returns(true);
                yield return new TestCaseData("encoding:UtF16", Encoding.Unicode).Returns(true);
                yield return new TestCaseData("encoding:UtF32", Encoding.UTF32).Returns(true);
                yield return new TestCaseData("encoding:UtF7", Encoding.UTF7).Returns(true);
                yield return new TestCaseData("e:utf-8", Encoding.UTF8).Returns(true);
                yield return new TestCaseData("e:utf8", Encoding.UTF8).Returns(true);
                yield return new TestCaseData("e:UtF8", Encoding.UTF8).Returns(true);
                yield return new TestCaseData("e:UtF16", Encoding.Unicode).Returns(true);
                yield return new TestCaseData("e:UtF32", Encoding.UTF32).Returns(true);
                yield return new TestCaseData("e:UtF7", Encoding.UTF7).Returns(true);

                yield return new TestCaseData("e:Midi-chlorian", null).Returns(false);
                yield return new TestCaseData("encoding:Midi-chlorian", null).Returns(false);
                yield return new TestCaseData("encoding:", null).Returns(false);
                yield return new TestCaseData("e:", null).Returns(false);
            }
        }

        [Test]
        [TestCaseSource("Load_Encoding_SetsDefaultEncoding_TestCases")]
        public bool Load_Encoding_SetsDefaultEncoding(string input, Encoding expected)
        {
            // Arrange
            var sut = new ArgumentsLoader();

            var args = input != null ? new[] {input} : new string[0];

            // Act
            var actual = sut.Load(args);

            // Assert
            Assert.That(sut.DefaultEncoding, Is.EqualTo(expected));
            return actual;
        }
    }
}
