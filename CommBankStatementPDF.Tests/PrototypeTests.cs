using CommBankStatementPDF.Business;
using System;
using NUnit.Framework;
using System.Diagnostics;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class PrototypeTests
    {
        [Test]
        public void dd()
        {
            var line0 = "30 Nov Direct Debit 373578 AGL RETAIL LTD";
            var line1 = "230001096364";
            var line2 = "AGL RETAIL LTD 51.60";

            var newProto = new Prototype() { AccountType = AccountType.StreamLine, Line0 = line0, Line1 = line1, Line2 = line2, SourceFile = "filename.pdf" };

            Trace.WriteLine(newProto.ToString());

            Assert.That(newProto.Biller, Is.EqualTo("Direct Debit 373578 AGL RETAIL LTD"));
        }
    }
}