﻿using System;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class HelperStreamline
    {
        public const string PAGE_HEADER = "Date Transaction Debit Credit Balance";
        public const string BEGIN_TRANSACTIONS = "Date Transaction Details Amount (A$)";

        public bool IsEndOfPage(string line)
        {
            // eg 23 May 2015- 23 Jun 2015

            var pattern = @"^\d{1,2} [A-z]{3} \d{4}";

            bool result = new Regex(pattern, RegexOptions.IgnoreCase).Match(line).Success;

            return result;
        }
    }
}