using CommBankStatementPDF.Business;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        //[Test, Ignore("obs")]
        //public void Full_Integration()
        //{
        //    Business.Data.DeleteAll();

        //    foreach (var account in new AccountType[] { AccountType.StreamLine, AccountType.VISA })
        //    {
        //        foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\" + account.ToString()), "*.pdf"))
        //        {
        //            var parser = new StatementParser(item, account);

        //            var file = new FileInfo(item);
        //            var expectedYear = Convert.ToInt32(file.Name.Substring(9, 4));

        //            parser.ReadFile();

        //            Trace.WriteLine(string.Format("FILE: {0} TRANSACTIONS: {1}", file.Name, parser.Transactions));
        //            Trace.WriteLine("******************************************************************************************");

        //            Business.Data.Save(parser.Transactions);
        //        }
        //    }
        //}

        [Test]
        public void Full_Integration_New_Parser()
        {
            Business.Data.DeleteAll();

            var sb = new StringBuilder();

            var trans = new List<Prototype>();

            foreach (var account in new AccountType[] { AccountType.StreamLine, AccountType.VISA })
            {
                foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\" + account.ToString()), "*.pdf"))
                {
                    var prototypes = IOHelper.GetPrototypes(item);

                    trans.AddRange(prototypes);

                    foreach (var p in prototypes)
                    {
                        Trace.WriteLine(p);

                        Assert.That(p.Biller, Is.Not.Empty);
                        Assert.That(p.AccountType, Is.Not.EqualTo(AccountType.Unknown));
                        Assert.That(p.Amount, Is.Not.Zero);
                    }
                }
            }

            Trace.WriteLine("FOUND: " + trans.Count);

            Business.Data.Save(trans);

            //File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "log.txt"), sb.ToString());
        }
    }
}