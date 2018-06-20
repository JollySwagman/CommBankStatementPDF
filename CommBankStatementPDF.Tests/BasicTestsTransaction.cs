using CommBankStatementPDF.Business;
using System;
using NUnit.Framework;

using System.Diagnostics;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsTransaction
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
            Assert.That(LineParser.GetAmountFromLine("10 Feb Payment Received, Thank You AU 100.00-"), Is.EqualTo(-100));
            Assert.That(LineParser.GetAmountFromLine("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"), Is.Not.Null);
        }

        //[Test]
        //public void ss()
        //{
        //    var d = new Transaction("23 May 2015 - 23 Jun 2015", 2015, AccountType.StreamLine);

        //    Assert.That(d.ParseSuccess, Is.False);
        //}

        //[Test]
        //public void ss2()
        //{
        //    var line1 = "23 May 2015 - 23 Jun 2015";

        //    var line2 = "23 May 2015- 23 Jun 2015";

        //    Assert.That(Transaction.IsCrap(line1), Is.True);
        //    Assert.That(Transaction.IsCrap(line2), Is.True);

        //    var tran = new Transaction(line1, 2015, AccountType.StreamLine);
        //    Assert.That(tran.ParseSuccess, Is.False);

        //    var tran2 = new Transaction(line2, 2015, AccountType.StreamLine);
        //    Assert.That(tran2.ParseSuccess, Is.False);
        //}
    }
}