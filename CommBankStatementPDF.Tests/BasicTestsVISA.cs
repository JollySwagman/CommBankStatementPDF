﻿using NUnit.Framework;
using System;
using System.IO;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsVISA
    {
        private string VISA_Statement20150623 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20150623.pdf");
        private string VISA_Statement20151218 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20151218.pdf");
        private string VISA_Statement20110121 = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\VISA\Statement20110121.pdf");

        //[Test]
        //public void Full_Integration()
        //{
        //    Business.Data.DeleteAll();

        //    foreach (var account in new string[] { "VISA", "Streamline" })
        //    {
        //        foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\" + account), "*.pdf"))
        //        {
        //            var parser = new StatementParser(item, AccountType.VISA);

        //            var file = new FileInfo(item);
        //            var expectedYear = Convert.ToInt32(file.Name.Substring(9, 4));

        //            parser.ReadFile();

        //            Trace.WriteLine(string.Format("FILE: {0} TRANSACTIONS: {1}", file.Name, parser.Transactions));
        //            Trace.WriteLine("******************************************************************************************");

        //            Business.Data.Save(parser.Transactions);
        //        }
        //    }
        //}

        //[Test]
        //public void Read_All_VISA_PDF_Files()
        //{
        //    foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\Streamline"), "*.pdf"))
        //    {
        //        var parser = new StatementParser(item, AccountType.VISA);

        //        var file = new FileInfo(item);
        //        var expectedYear = Convert.ToInt32(file.Name.Substring(9, 4));

        //        parser.ReadFile();

        //        Trace.WriteLine(string.Format("FILE: {0} TRANSACTIONS: {1}", file.Name, parser.Transactions));
        //        Trace.WriteLine("******************************************************************************************");

        //        Assert.That(parser.Transactions.Count, Is.GreaterThan(0));

        //        //Trace.WriteLine(string.Format("************{0} {1}", file.Name, parser.Year));
        //        foreach (var tran in parser.Transactions)
        //        {
        //            //Trace.WriteLine(tran);
        //            //    Trace.WriteLine(string.Format("TEST: {0}\t{1}\t{2}", tran.Date, tran.Amount, tran.Biller));

        //            //                    Assert.That(tran.Amount, Is.Not.EqualTo(0));
        //            Assert.That(tran.SourceFile, Is.EqualTo(new FileInfo(item).Name));
        //            Assert.That(tran.Source, Is.Not.Empty);
        //            Assert.That(tran.Amount, Is.LessThan(3000));
        //        }

        //        Assert.That(parser.Year, Is.EqualTo(expectedYear));

        //        // TEMP!
        //        // To db
        //        if (true)
        //        {
        //            Business.Data.DeleteAll();
        //            Business.Data.Save(parser.Transactions);
        //        }
        //    }
        //}

        //[Test]
        //public void GetYear()
        //{
        //    //var contents = IOHelper.ReadPdfFile(testFilename2);

        //    var parser = new StatementParser(VISA_Statement20151218, AccountType.VISA);
        //    parser.ReadFile();
        //    //contents = parser.GetTransactions(contents);

        //    Assert.That(parser.Year, Is.GreaterThan(1900));
        //}

        //[Test]
        //public void testFilename3_Should_Have_84_Transactions()
        //{
        //    var parser = new StatementParser(VISA_Statement20150623, AccountType.VISA);
        //    parser.ReadFile();

        //    var sb = new StringBuilder();

        //    foreach (var item in parser.Transactions)
        //    {
        //        Assert.That(item.Date.Year > 1900);
        //        //Trace.WriteLine(item);
        //        sb.AppendLine(item.ToString());
        //    }

        //    Trace.WriteLine("COUNT: " + parser.Transactions.Count);
        //    Trace.WriteLine("TOTAL: " + parser.GetTransactionTotal());
        //    Assert.That(parser.Transactions.Count, Is.EqualTo(84));
        //    //Assert.That(parser.GetTransactionTotal(), Is.EqualTo(7950.78));

        //    SaveTransactions(parser.Transactions);
        //}

        //[Test]
        //public void ParseToTransaction_New_Format()
        //{
        //    var parser = new StatementParser(VISA_Statement20151218, AccountType.VISA);
        //    parser.ReadFile();

        //    var trans = "07 Apr Bunnings 370000 Alexandria 160.00";

        //    trans = "14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions";

        //    var result = new Transaction(trans, 2001, AccountType.VISA);

        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Amount, Is.GreaterThan(0));

        //    Trace.WriteLine("*****************************************************");
        //    foreach (var item in parser.Transactions)
        //    {
        //        Assert.That(item.Date.Year > 1900);
        //        Trace.WriteLine(item);
        //    }

        //    Trace.WriteLine(parser.ToString());
        //}

        //[Test]
        //public void ParseToTransaction_Multiline()
        //{
        //    var parser = new StatementParser(AccountType.VISA);

        //    var lines = new List<string>(new string[] { "29 May Pu 52429 Groznjan Groznjan", "##5153           2000.00CROATIAN KUNAHR", "379.31Transactions" });

        //    var result = new Transaction(lines, 2001, AccountType.VISA);

        //    Trace.WriteLine(result);

        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Amount, Is.EqualTo(379.31));
        //}

        //[Test]
        //public void ParseToTransaction_Old_Format()
        //{
        //    var parser = new StatementParser(VISA_Statement20110121, AccountType.VISA);
        //    parser.ReadFile();

        //    Trace.WriteLine(parser.Source);

        //    Assert.That(parser.Transactions, Is.Not.Null);
        //    Assert.That(parser.Transactions.Count, Is.EqualTo(4));

        //    foreach (var item in parser.Transactions)
        //    {
        //        Assert.That(item.Date.Year > 1900);
        //        Trace.WriteLine(item);
        //    }

        //    Trace.WriteLine(parser.ToString());

        //    SaveTransactions(parser.Transactions);
        //}
    }
}