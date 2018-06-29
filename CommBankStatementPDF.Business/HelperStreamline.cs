using System;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class HelperVISA
    {
        public const string PAGE_HEADER = "Date Transaction Debit Credit Balance";
        public const string BEGIN_TRANSACTIONS = "OPENING BALANCE";
        public const string END_TRANSACTIONS = "CLOSING BALANCE";

        public bool xIsEndOfPage(string line)
        {
            // eg 23 May 2015- 23 Jun 2015

            var pattern = @"^\d{1,2} [A-z]{3} \d{4}";

            bool result = new Regex(pattern, RegexOptions.IgnoreCase).Match(line).Success;

            return result;
        }
    }
}