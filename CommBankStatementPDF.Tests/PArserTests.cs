using CommBankStatementPDF.Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void ReadPdfFileToPages_()
        {
            //Trace.WriteLine(IOHelper.ReadPdfFileToPages(testFilename2));

            var text = IOHelper.ReadPdfFileToPages(testFilename2);

            var lines = IOHelper.XXXParser2(text);

            var transLines = new List<Prototype>();

            var newProto = new Prototype();
            var lineCount = 0;

            foreach (var item in lines)
            {
                Trace.WriteLine(item);
                var date = Transaction.GetDateFromLine(item, 2000);
                if (date.HasValue)
                {
                    // save the previous and start a new one
                    if (newProto != null)
                    {
                        transLines.Add(newProto);
                    }
                    newProto = new Prototype() { Date = date.Value, Line0 = item };
                }
                else
                {
                    lineCount++;
                    if (lineCount == 0)
                    {
                        newProto.Line0 = item;
                    }
                    if (lineCount == 1)
                    {
                        newProto.Line1 = item;
                    }
                    if (lineCount == 2)
                    {
                        newProto.Line2 = item;
                    }
                }
            }
        }
    }
}