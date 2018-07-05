using NUnit.Framework;
using System;
using System.Diagnostics;

namespace CommBankStatementPDF.Tests
{
    [TestFixture]
    public class DataTests : TestBase
    {
        [Test]
        public void Remove_All_Rows()
        {
            // danger!            Business.Data.DeleteAll();
        }
        [Test]
        public void GetPassword()
        {
            Trace.WriteLine("PW: " + GetZipPassword());
        }
    }
}