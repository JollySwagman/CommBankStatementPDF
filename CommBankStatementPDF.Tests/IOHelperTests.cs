using CommBankStatementPDF.Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class IOHelperTests
    {
        private const decimal MAX_AMOUNT = 3000;

        private string testFilename0 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20151218.pdf");

        //private string testFilenameOldFormat = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\Streamline\Statement20100831.pdf");
        private string testFilename1 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\StreamLine\Statement20160131.pdf");

        private string testFilename2 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\StreamLine\Statement20161207.pdf");

        //[Test]
        //public void Aspose()
        //{
        //}

        [Test]
        public void Get_Pages()
        {
            Business.Data.DeleteAll();

            var IOHelper = new IOHelper();

            //var pages = IOHelper.ReadPdfFileToPages(testFilename2);

            //var prototypes = IOHelper.GetLinesFromPages(pages);
            var prototypes = IOHelper.GetPrototypes(testFilename2);

            foreach (var item in prototypes)
            {
                Trace.WriteLine(item);
            }

            Assert.That(prototypes.Count, Is.GreaterThan(0));

            Business.Data.Save(prototypes);
        }

        [Test]
        public void ParseLine_2()
        {
            //Return 01/11/16|Loan Repayment|LN REPAY 241094909|Value Date: 01/11/2016  $486.00|
            //Return 01/11/16|Loan Repayment|LN REPAY 241094909|Value Date: 01/11/2016  $486.00|

            var lines = IOHelper.GetLinesFromPages(new List<string>(new string[] { "14 Nov Direct Debit 044952 OPTUS", "10478488000192 97.20 $ $1,912.00 DR" }), true);
            ParseLine_Tester(lines);
        }

        [Test]
        public void ParseLine()
        {
            var lines = IOHelper.GetLinesFromPages(new List<string>(new string[] { "14 Nov Direct Debit 044952 OPTUS", "10478488000192 97.20 $ $1,912.00 DR" }), true);
            // Direct Debit 044952 OPTUS10478488000192 97.20 $ $1,912.00 DR

            var parser = new NewParser();

            //var result = parser.GetPrototypesFromLines(new Queue<string>(lines));
            var result = parser.GetPrototypesFromLines(lines, 2016, AccountType.StreamLine, "xxx");

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        public void ParseLine_Tester(List<string> lines)
        {
            //var lines = IOHelper.GetLinesFromPages(new List<string>(new string[] { "14 Nov Direct Debit 044952 OPTUS", "10478488000192 97.20 $ $1,912.00 DR" }), true);
            // Direct Debit 044952 OPTUS10478488000192 97.20 $ $1,912.00 DR

            var parser = new NewParser();

            //var result = parser.GetPrototypesFromLines(new Queue<string>(lines));
            var result = parser.GetPrototypesFromLines(lines, 2016, AccountType.StreamLine, "xxx");

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        [Test]
        public void GetAmountFromLine()
        {
            var result = LineParser.GetAmountFromLine("10478488000192 97.20 $ $1,912.00 DR");

            Assert.That(result, Is.EqualTo(97.20));
        }

        [Test]
        public void StripBalance_()
        {
            //"Direct Debit 044952 OPTUS | 10478488000192 97.20 $ $1,912.00 DR "

            var result = LineParser.StripBalance("10478488000192 97.20 $ $1,912.00 DR ");

            Assert.That(result, Is.EqualTo("10478488000192 97.20"));
        }
    }
}