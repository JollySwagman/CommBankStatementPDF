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
            var x = new Transaction("25 Apr 2017 - 24 May 2017", 2001);

            Assert.That(Transaction.IsTransaction("25 Apr 2017 - 24 May 2017"), Is.False);
            Assert.That(Transaction.IsTransaction("53 NORTHWARD ST"), Is.False);
            Assert.That(Transaction.IsTransaction("01 Jan xxxxx"), Is.True);
            Assert.That(Transaction.IsTransaction("Hello world! 01 Jan"), Is.False);
            Assert.That(Transaction.IsTransaction("HELLO!"), Is.False);
            Assert.That(Transaction.IsTransaction("Statement Period 24 Mar 2017 - 24 Apr 2017"), Is.False);
            Assert.That(Transaction.IsTransaction("24 Mar 2017- 24 Apr 2017"), Is.False);
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

            var result = new Transaction(trans, 2001);

            Assert.That(result, Is.Not.Null);

            Trace.WriteLine(contents);
            Trace.WriteLine("*****************************************************");
            foreach (var item in parser.Transactions)
            {
                Trace.WriteLine(item);
            }
        }

        [Test]
        public void IsCrap()
        {
            
            Assert.That(Transaction.IsCrap("25 Apr 2017 - 24 May 2017"), Is.True);
            Assert.That(Transaction.IsCrap("Statement Period 24 Mar 2017 - 24 Apr 2017"), Is.True);
            Assert.That(Transaction.IsCrap("25 Apr 2017 - 24 May 2017"), Is.True);
            Assert.That(Transaction.IsCrap("24 Mar 2017- 24 Apr 2017"), Is.True);
        }
    }
}