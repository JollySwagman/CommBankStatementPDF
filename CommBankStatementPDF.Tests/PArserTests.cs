using CommBankStatementPDF.Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System;

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

            parser.Parser2(pages);

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
    }
}