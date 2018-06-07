using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class LineParser
    {
        public static decimal? GetAmountFromLine(string line)
        {
            decimal? result = null;

            line = TrimEndAlpha(line);

            var num = line.Substring(line.LastIndexOf(' ') + 1);

            if (num.Contains(".") && decimal.TryParse(num, out decimal number))
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

        /// <summary>
        /// eg "( $336.22 CR"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimEndBalance(string line)
        {
            string result = line;

            //var xxx = new Regex(@"\$[-,0-9]+ CR$|DR$", RegexOptions.IgnoreCase).Match(line);

            //            var xxx = new Regex(@"[-,0-9\.]+ CR$|DR$", RegexOptions.IgnoreCase).Match(line);

            //var xxx = new Regex(@"\( [-,0-9\.]+ CR$|DR$", RegexOptions.IgnoreCase).Match(line);

            var pattern = @" \$[-,0-9\.]+ CR|DR";

            pattern = @" \( \$[-,0-9\.]+ [C|D]R";   //@" \$[-,0-9\.]+ [C|D]R";

            var xxx = new Regex(pattern, RegexOptions.IgnoreCase).Match(line);

            //            Trace.WriteLine(line);

            if (xxx.Success)
            {
                result = line.Replace(xxx.Value, "");
                //Trace.WriteLine("*****  " + line);
                //Trace.WriteLine("*****  " + result);

                //if (resul)
                //{
                //}
            }
            else
            {
                Trace.WriteLine("-----  " + line);
            }

            return result;
        }

        public static bool IsNumeric(string character)
        {
            return Int32.TryParse(character, out int x);
        }
    }
}