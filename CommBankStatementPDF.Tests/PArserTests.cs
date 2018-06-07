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
        public void Get_Pages()
        {
            var pages = IOHelper.ReadPdfFileToPages(testFilename2);

            var parser = new StatementParser(testFilename2, StatementParser.AccountType.StreamLine);

            //parser.Parser2(pages);

            Trace.WriteLine(parser.FilteredSource);
        }

        [Test]
        public void Filter_Lines()
        {
            //var src = "15 Jun NETBANK TFR\nIRM Pay 03,607.00\n15 Jun COMMONWEALTH BNK";

            //var lines = new List<string>(new string[] { @"15 Jun NETBANK TFR", @"IRM Pay 03,607.00", @"15 Jun COMMONWEALTH BNK" });

            //parser.Source = src;

            //var trans = new Transaction(lines, 2010, StatementParser.AccountType.StreamLine);
            //Trace.WriteLine(trans);
            //Assert.That(trans.Amount, Is.EqualTo(10));
        }

        [Test]
        public void ReadPdfFileToPages_a()
        {
            var result = IOHelper.GetPrototypes(testFilename2);

            foreach (var item in result)
            {
                Trace.WriteLine(item);
            }
        }

        //[Test]
        //public void ReadPdfFileToPages_()
        //{
        //    //Trace.WriteLine(IOHelper.ReadPdfFileToPages(testFilename2));

        //    var text = IOHelper.ReadPdfFileToPages(testFilename2);

        //    var lines = IOHelper.XXXParser2(text);

        //    var result = IOHelper.GetPrototypes(lines);

        //    foreach (var item in result)
        //    {
        //        //Trace.WriteLine(item);
        //    }
        //}

        //

        [Test]
        public void TrimEndBalance_()
        {
            //Assert.That(LineParser.TrimEndBalance("14 Apr"), Is.EqualTo("14 Apr"));

            //26 Apr Church St Medical Newtown AU 72.00 ( $492.44 DR
            Assert.That(LineParser.TrimEndBalance("26 Apr Church St Medical Newtown AU 72.00 ( $492.44 DR"), Is.EqualTo("26 Apr Church St Medical Newtown AU 72.00"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 CR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 DR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 XR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 XR"));
            Assert.That(LineParser.TrimEndBalance("14 Apr Transfer to xx1119 CommBank app 50.00 ( $9,279.46 CR"), Is.EqualTo("14 Apr Transfer to xx1119 CommBank app 50.00"));
        }
    }
}