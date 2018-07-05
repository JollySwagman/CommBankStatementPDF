using System;
using NUnit.Framework;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class TestBase
    {
        public static string testFilesFolder = @"C:\Users\Boss\Documents\BankStatements";

        public readonly string testFilename2 = Path.Combine(testFilesFolder, @"TestFiles\Streamline\Statement20180430.pdf");
        public readonly string testFilenameOldFormat = Path.Combine(testFilesFolder, @"TestFiles\Streamline\Statement20100831.pdf");

        public string testFilename0 = Path.Combine(testFilesFolder, @"TestFiles\VISA\Statement20151218.pdf");

        //public  string testFilenameOldFormat = Path.Combine(testFilesFolder, @"TestFiles\Streamline\Statement20100831.pdf");
        public string testFilename1 = Path.Combine(testFilesFolder, @"TestFiles\All\StreamLine\Statement20160131.pdf");

        public string testFilename3 = Path.Combine(testFilesFolder, @"TestFiles\All\StreamLine\Statement20161207.pdf");


        public string GetZipPassword()
        {
            return Environment.GetEnvironmentVariable("DevPassword");
        }



    }
}