using CommBankStatementPDF.Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CommBankStatementPDF.Tests
{
    //  [TestFixture, Category("IntegrationTests")]
    public class IntegrationTests : TestBase
    {
        [Test]
        public void Full_Integration_New_Parser()
        {

            var sb = new StringBuilder();

            var trans = new List<Prototype>();

            var IOHelper = new IOHelper();

            foreach (var account in new AccountType[] { AccountType.StreamLine, AccountType.VISA })
            {
                // foreach (var item in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\All\" + account.ToString()), "*.pdf"))
                foreach (var item in Directory.GetFiles(Path.Combine(testFilesFolder, @"TestFiles\All\" + account.ToString()), "*.pdf"))
                {
                    var prototypes = IOHelper.GetPrototypes(item);

                    trans.AddRange(prototypes);

                    foreach (var p in prototypes)
                    {
                        p.GetAmount();
                        Trace.WriteLine(p);

                        Assert.That(p.Biller, Is.Not.Empty);
                        Assert.That(p.AccountType, Is.Not.EqualTo(AccountType.Unknown));
                        Assert.That(p.Amount, Is.Not.Zero);
                    }
                }
            }

            Trace.WriteLine("FOUND: " + trans.Count);

            //Trace.WriteLine("Writing to DB");
            //Business.Data.DeleteAll();
            //Business.Data.Save(trans);
            //Trace.WriteLine("Finished writing to DB");

            //File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "log.txt"), sb.ToString());
        }
    }
}