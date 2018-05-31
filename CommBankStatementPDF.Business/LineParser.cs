using System;

namespace CommBankStatementPDF.Business
{
    public class LineParser
    {
        public static decimal? GetAmountFromLine(string line)
        {
            decimal? result = null;

            line = TrimEndAlpha(line);

            if (decimal.TryParse(line.Substring(line.LastIndexOf(' ') + 1), out decimal number))
            {
                result = number;
            }
            return result;
        }

        public static string TrimEndAlpha(string value)
        {
            string result = value;
            if (value != null && !string.IsNullOrEmpty(value))
            {
                for (int i = value.Length - 1; i >= 0; i--)
                {
                    if (IsNumeric(Convert.ToString(value[i])) || value[i] == '-')
                    {
                        //Trace.WriteLine(value[i] + " (NUMERIC)");
                        break;
                    }
                    else
                    {
                        //Trace.WriteLine(value[i] + " (NOT NUMERIC)");
                        result = value.Substring(0, i);
                    }
                }
            }
            return result;
        }

        public static bool IsNumeric(string character)
        {
            return Int32.TryParse(character, out int x);
        }
    }
}