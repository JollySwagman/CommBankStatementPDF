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
        public void Amount_Is_Parsed_Correctly()
        {
            var line0 = "30 Nov Direct Debit 373578 AGL RETAIL LTD";
            var line1 = "230001096364";
            var line2 = "AGL RETAIL LTD 51.60";

            var obj = new Prototype() { AccountType = AccountType.StreamLine, Line0 = line0, Line1 = line1, Line2 = line2, SourceFile = "filename.pdf" };

            var amt = obj.GetAmount();

            Trace.WriteLine(amt.GetValueOrDefault());
            Trace.WriteLine(obj.ToString());

            Assert.That(obj.Biller, Is.EqualTo("Direct Debit 373578 AGL RETAIL LTD"));
            Assert.That(obj.Amount, Is.EqualTo(51.60));
        }

        [Test]
        public void Empty_Can_Return_Correct_Amount()
        {
            var obj = new Prototype();

            Trace.WriteLine(obj.Amount);
            Assert.That(obj.Amount, Is.EqualTo(0));
        }
    }
}