using CommBankStatementPDF.Business;
using System;
using NUnit.Framework;

using System;
using System.Diagnostics;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsTransaction
    {
        [Test]
        public void TrimEndAlpha_()
        {
            Trace.WriteLine(Transaction.TrimEndAlpha("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"));

            Assert.That(Transaction.TrimEndAlpha("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"), Is.EqualTo("14 Feb Bunnings 370000 AlexandriaAU 157.25"));

            Assert.That(Transaction.TrimEndAlpha("abc"), Is.EqualTo(""));

            Assert.That(Transaction.TrimEndAlpha("123abc"), Is.EqualTo("123"));

            Assert.That(Transaction.TrimEndAlpha(""), Is.EqualTo(""));
            Assert.That(Transaction.TrimEndAlpha(null), Is.EqualTo(null));
            Assert.That(Transaction.TrimEndAlpha("123"), Is.EqualTo("123"));
            Assert.That(Transaction.TrimEndAlpha("abc123"), Is.EqualTo("abc123"));
        }

        [Test]
        public void GetAmountFromLine_()
        {
            Assert.That(Transaction.GetAmountFromLine("10 Feb Payment Received, Thank You AU 100.00-"), Is.EqualTo(-100));

            Assert.That(Transaction.GetAmountFromLine("14 Feb Bunnings 370000 AlexandriaAU 157.25Transactions"), Is.Not.Null);
        }





    }
}