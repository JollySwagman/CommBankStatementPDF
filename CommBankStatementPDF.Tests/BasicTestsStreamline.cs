using System;
using CommBankStatementPDF.Business;
using NUnit.Framework;

using System;
using System.Diagnostics;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsStreamline
    {
        private string testFilename2 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\Streamline\Statement20180430.pdf");
        private string testFilenameOldFormat = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\Streamline\Statement20100831.pdf");


        [Test]
        public void Read_All_Streamline_PDF_Files()
        {
            var parser = new StatementParser();

            foreach (var item in Directory.GetFiles(@"C:\Users\Boss\Google Drive\Tax\Streamline", "*.pdf"))
            {
                var file = new FileInfo(item);
                var expectedYear = Convert.ToInt32(file.Name.Substring(9, 4));

                parser.ReadFile(file.FullName);

                Trace.WriteLine(string.Format("************{0} {1}", file.Name, parser.Year));
                foreach (var tran in parser.Transactions)
                {
                    Trace.WriteLine(tran);
                }

                Assert.That(parser.Year, Is.EqualTo(expectedYear));
            }
        }

        [Test]
        public void GetYear()
        {
            //var contents = IOHelper.ReadPdfFile(testFilename2);

            var parser = new StatementParser();
            parser.ReadFile(testFilename2);
            //contents = parser.GetTransactions(contents);

            Assert.That(parser.Year, Is.GreaterThan(1900));
        }

        [Test]
        public void ParseToTransaction_New_Format()
        {
            var parser = new StatementParser();
            parser.ReadFile(testFilename2);

            var trans = "07 Apr Bunnings 370000 Alexandria 160.00";
            var result = new Transaction(trans, 2001);

            Assert.That(result, Is.Not.Null);

            Trace.WriteLine("*****************************************************");
            foreach (var item in parser.Transactions)
            {
                Assert.That(item.Date.Year > 1900);
                Trace.WriteLine(item);
            }
        }

        [Test]
        public void ParseToTransaction_Old_Format()
        {
            var parser = new StatementParser();
            parser.ReadFile(testFilenameOldFormat);

            //Trace.WriteLine(parser.Source);

            Assert.That(parser.Transactions, Is.Not.Null);
            Assert.That(parser.Transactions.Count, Is.EqualTo(61));
            
            foreach (var item in parser.Transactions)
            {
                Assert.That(item.Date.Year > 1900);
                //Trace.WriteLine(item);
            }
        }

        //[Test]
        //public void IsTransaction_Identifies_Line_Begins_With()
        //{
        //    var x = new Transaction("25 Apr 2017 - 24 May 2017", 2001);

        //    Assert.That(Transaction.IsTransaction("7 Jan 749405207P98NU9TR Skip Bins Online Pty Constitution AU 392.40"), Is.True);

        //    Assert.That(Transaction.IsTransaction("25 Apr 2017 - 24 May 2017"), Is.False);
        //    Assert.That(Transaction.IsTransaction("53 NORTHWARD ST"), Is.False);
        //    Assert.That(Transaction.IsTransaction("01 Jan xxxxx"), Is.True);
        //    Assert.That(Transaction.IsTransaction("Hello world! 01 Jan"), Is.False);
        //    Assert.That(Transaction.IsTransaction("HELLO!"), Is.False);
        //    Assert.That(Transaction.IsTransaction("Statement Period 24 Mar 2017 - 24 Apr 2017"), Is.False);
        //    Assert.That(Transaction.IsTransaction("24 Mar 2017- 24 Apr 2017"), Is.False);
        //}

        //[Test]
        //public void IOHelper_Reads_File()
        //{
        //    var parser = new StatementParser();
        //    parser.ReadFile(testFilename);
        //   var result = parser.GetTransactions();

        //    Trace.WriteLine(result);
        //}

        //[Test]
        //public void IsCrap()
        //{
        //    Assert.That(Transaction.IsCrap("25 Apr 2017 - 24 May 2017"), Is.True);
        //    Assert.That(Transaction.IsCrap("Statement Period 24 Mar 2017 - 24 Apr 2017"), Is.True);
        //    Assert.That(Transaction.IsCrap("25 Apr 2017 - 24 May 2017"), Is.True);
        //    Assert.That(Transaction.IsCrap("24 Mar 2017- 24 Apr 2017"), Is.True);
        //}
    }
}