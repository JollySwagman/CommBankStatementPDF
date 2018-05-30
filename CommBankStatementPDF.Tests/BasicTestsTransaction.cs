using CommBankStatementPDF.Business;
using System;
using NUnit.Framework;

using System;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class BasicTestsTransaction
    {
        [Test]
        public void Trim_Alpha()
        {
            Assert.That(Transaction.TrimEndAlpha("abc"), Is.EqualTo("abc")); // contentious...

            Assert.That(Transaction.TrimEndAlpha("123abc"), Is.EqualTo("123"));

            Assert.That(Transaction.TrimEndAlpha(""), Is.EqualTo(""));
            Assert.That(Transaction.TrimEndAlpha(null), Is.EqualTo(null));
            Assert.That(Transaction.TrimEndAlpha("123"), Is.EqualTo("123"));
            Assert.That(Transaction.TrimEndAlpha("abc123"), Is.EqualTo("abc123"));


        }
    }
}