using System;
using CommBankStatementPDF.Business;
using NUnit.Framework;

using System;
using System.Diagnostics;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsVISA
    {
        private string testFilename2 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20151218.pdf");
        private string testFilenameOldFormat = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20110121.pdf");

        [Test]
        public void Read_All_VISA_PDF_Files()
        {
            var parser = new StatementParser();

            foreach (var item in Directory.GetFiles(@"C:\Users\Boss\Google Drive\Tax\VISA", "*.pdf"))
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

            Trace.WriteLine(parser.Source);

            Assert.That(parser.Transactions, Is.Not.Null);
            Assert.That(parser.Transactions.Count, Is.EqualTo(4));

            foreach (var item in parser.Transactions)
            {
                Assert.That(item.Date.Year > 1900);
                Trace.WriteLine(item);
            }
        }
    }
}