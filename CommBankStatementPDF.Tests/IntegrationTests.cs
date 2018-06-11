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
        [Test]
        public void Full_Integration()
        {
            Business.Data.DeleteAll();

            foreach (var account in new StatementParser.AccountType[] { StatementParser.AccountType.StreamLine, StatementParser.AccountType.VISA })
            {
                foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\" + account.ToString()), "*.pdf"))
                {
                    var parser = new StatementParser(item, account);

                    var file = new FileInfo(item);
                    var expectedYear = Convert.ToInt32(file.Name.Substring(9, 4));

                    parser.ReadFile();

                    Trace.WriteLine(string.Format("FILE: {0} TRANSACTIONS: {1}", file.Name, parser.Transactions));
                    Trace.WriteLine("******************************************************************************************");

                    Business.Data.Save(parser.Transactions);
                }
            }
        }

        [Test]
        public void Full_Integration_New_Parser()
        {
            Trace.WriteLine("Full_Integration_New_Parser()");

            var sb = new StringBuilder();

            var trans = new List<Transaction>();

            foreach (var account in new StatementParser.AccountType[] { StatementParser.AccountType.StreamLine, StatementParser.AccountType.VISA })
            {
                foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\" + account.ToString()), "*.pdf"))
                {
                    var prototypes = IOHelper.GetPrototypes(item);

                    //var t = new Transaction()

                    foreach (var p in prototypes)
                    {
                        //sb.AppendLine(p.ToString());
                        Trace.WriteLine(p);
                    }
                }
            }

            //File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "log.txt"), sb.ToString());
        }
    }
}