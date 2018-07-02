using CommBankStatementPDF.Business;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private const decimal MAX_AMOUNT = 3000;

        private string testFilename2 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\Streamline\Statement20180430.pdf");
        private string testFilenameOldFormat = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\Streamline\Statement20100831.pdf");

        [Test]
        public void ReadPdfFileToPages_()
        {
            var IOHelper = new IOHelper();

            var result = IOHelper.GetPrototypes(testFilename2);

            foreach (var item in result)
            {
                Trace.WriteLine(item);
            }
        }

        [Test]
        public void TrimEndBalance_()
        {
            //Assert.That(LineParser.TrimEndBalance("14 Apr"), Is.EqualTo("14 Apr"));

            Assert.That(LineParser.TrimEndBalance("28 Feb Transfer from xx4909 CommBank app $1,000.00 $904.81 DR"), Is.EqualTo("28 Feb Transfer from xx4909 CommBank app $1,000.00"));
            Assert.That(LineParser.TrimEndBalance("003059 110.00 ) $959.48 CR"), Is.EqualTo("003059 110.00"));
            Assert.That(LineParser.TrimEndBalance("26 Apr Church St Medical Newtown AU 72.00 ( $492.44 DR"), Is.EqualTo("26 Apr Church St Medical Newtown AU 72.00"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 CR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 DR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 XR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 XR"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 CR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00"));
        }
    }
}