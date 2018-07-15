using CommBankStatementPDF.Business;
using System;
using NUnit.Framework;

using System.Diagnostics;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsTransaction : TestBase
    {
        [Test]
        public void TrimEndAlpha_()
        {
            Trace.WriteLine(LineParser.TrimEndAlpha("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"));

            Assert.That(LineParser.TrimEndAlpha("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"), Is.EqualTo("14 Feb Bunnings 370000 AlexandriaAU 157.25"));
            Assert.That(LineParser.TrimEndAlpha("abc"), Is.EqualTo(""));
            Assert.That(LineParser.TrimEndAlpha("123abc"), Is.EqualTo("123"));
            Assert.That(LineParser.TrimEndAlpha(""), Is.EqualTo(""));
            Assert.That(LineParser.TrimEndAlpha(null), Is.EqualTo(null));
            Assert.That(LineParser.TrimEndAlpha("123"), Is.EqualTo("123"));
            Assert.That(LineParser.TrimEndAlpha("abc123"), Is.EqualTo("abc123"));
        }

        [Test]
        public void GetAmountFromLine_()
        {
            //||||

            Assert.That(LineParser.GetAmountFromLine("Wdl ATM ANZ 115 PITT ST BRANCH SYDNEY 330.00 $ $4,650.44 CR"), Is.EqualTo(330));

            //Assert.That(LineParser.StripBalance("10 Feb Payment Received, Thank You AU 100.00-"), Is.EqualTo("10 Feb Payment Received, Thank You AU 100.00-"));

            Assert.That(LineParser.GetAmountFromLine("10 Feb Payment Received, Thank You AU 100.00-"), Is.EqualTo(-100));
            Assert.That(LineParser.GetAmountFromLine("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"), Is.Not.Null);

            //20 Nov Transfer from xx4909 CommBank app $500.00

            var x = LineParser.GetAmountFromLine(@"20 Nov Transfer from xx4909 CommBank app $500.00");

            Assert.That(x.HasValue);
            Assert.That(x.Value, Is.EqualTo(500));

            Assert.That(LineParser.GetAmountFromLine(@"to June 30, 2011 is 0.16 0").GetValueOrDefault(), Is.EqualTo(.16));
        }

        [Test]
        public void ParseToTransaction_Old_Format()
        {
            var IOHelper = new IOHelper() { Verbose = true };

            var prototypes = IOHelper.GetPrototypes(testFilenameOldFormat);

            Trace.WriteLine("");

            Assert.That(prototypes, Is.Not.Null);

            foreach (var item in prototypes)
            {
                Assert.That(item.Date.Year > 1900);
                Assert.That(item.AccountType, Is.EqualTo(AccountType.StreamLine));
                Trace.WriteLine(item);
            }

            // TODO: uncomment Assert.That(prototypes.Count, Is.EqualTo(74));
        }
    }
}