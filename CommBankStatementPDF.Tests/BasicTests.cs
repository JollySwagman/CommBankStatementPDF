using System;
using CommBankStatementPDF.Business;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTests
    {
        private string filename = Path.Combine(TestContext.CurrentContext.TestDirectory, "Test01.pdf");

        [Test]
        public void IsTransaction_Identifies_Line_Begins_With()
        {
            Assert.That(StatementParser.IsTransaction("53 NORTHWARD ST"), Is.False);

            Assert.That(StatementParser.IsTransaction("01 Jan"), Is.True);
            Assert.That(StatementParser.IsTransaction("Hello world! 01 Jan"), Is.False);
            Assert.That(StatementParser.IsTransaction("HELLO!"), Is.False);
        }

        [Test]
        public void ReadPdfText()
        {
            var result = PDFParser.ReadPdfFile(filename);

            var parser = new StatementParser();
            result = parser.GetTransactions(result);

            Trace.WriteLine(result);
        }

        [Test]
        public void ParseToTransaction()
        {
            var parser = new StatementParser();

            var contents = PDFParser.ReadPdfFile(filename);
            contents = parser.GetTransactions(contents);

            var trans = "07 Apr Bunnings 370000 Alexandria 160.00";

            var result = parser.ParseTransaction(trans);

            Assert.That(result, Is.Not.Null);

            Trace.WriteLine(result);
        }
    }
}