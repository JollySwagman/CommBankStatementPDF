using CommBankStatementPDF.Business;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class IOHelperTests
    {
        private const decimal MAX_AMOUNT = 3000;

        private string testFilename2 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20151218.pdf");
        //private string testFilenameOldFormat = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\Streamline\Statement20100831.pdf");

        [Test]
        public void Get_Pages()
        {
            //var pages = IOHelper.ReadPdfFileToPages(testFilename2);

            //var prototypes = IOHelper.GetLinesFromPages(pages);
            var prototypes = IOHelper.GetPrototypes(testFilename2);

            foreach (var item in prototypes)
            {
                Trace.WriteLine(item);
            }

            Assert.That(prototypes.Count, Is.GreaterThan(0));
        }
    }
}