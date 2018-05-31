using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommBankStatementPDF.Business
{
public    class StreamLine
    {


        public static bool IsBracketLine(string line)
        {
            return line.Count(f => f == ')') == 1; ;
        }

            public static decimal GetAmountFromBracketLine(string line)
            {
            var line2 = line.Substring(0, line.IndexOf(')'));
            //this.Amount = GetAmountFromLine(line2).GetValueOrDefault();
            return 0;
        }

    }
}
