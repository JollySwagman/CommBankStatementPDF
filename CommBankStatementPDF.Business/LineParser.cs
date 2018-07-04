using System;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class LineParser
    {
        public const int MIN_LINE_LENGTH = 6;

        public static DateTime? GetDateFromLine(string value, int year)
        {
            DateTime? result = null;
            Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);

            if (string.IsNullOrWhiteSpace(value) == false && value.Length >= MIN_LINE_LENGTH)
            {
                Match m = r.Match(value.Substring(0, MIN_LINE_LENGTH));

                if (!string.IsNullOrEmpty(m.Value))
                {
                    var month = m.Value.Split(' ')[1];     // had a gutful of regex now
                    var day = m.Value.Substring(0, 2);

                    if (DateTime.TryParse("01-" + month + "-2000", out DateTime monthIndex))
                    {
                        int dayIndex = Convert.ToInt32(day);
                        result = new DateTime(year, monthIndex.Month, dayIndex);
                    }
                }
            }

            return result;
        }

        public static string StripLeadingDate(string line)
        {
            string result = line;
            Regex r = new Regex(@"^\d{1,2} [A-z]{3} ", RegexOptions.IgnoreCase);

            if (string.IsNullOrWhiteSpace(line) == false && line.Length >= MIN_LINE_LENGTH)
            {
                //Match m = r.Match(line.Substring(0, MIN_LINE_LENGTH));
                Match m = r.Match(line);
                result = line.Substring(m.Length);
            }

            return result;
        }

        public static bool IsCrap(string line)
        {
            // eg 23 May 2015- 23 Jun 2015

            var pattern = @"^\d{1,2} [A-z]{3} \d{4}";

            pattern = @"^\d{1,2} [A-z]{3} \d{4} - \d{1,2} [A-z]{3} \d{4}$";

            var pattern2 = @"^\d{1,2} [A-z]{3} \d{4}- \d{1,2} [A-z]{3} \d{4}$";

            var pattern3 = @"^\d{1,2} ([^\s]+) \d{4}$";

            bool result = new Regex(pattern, RegexOptions.IgnoreCase).Match(line).Success || new Regex(pattern3, RegexOptions.IgnoreCase).Match(line).Success || new Regex(pattern2, RegexOptions.IgnoreCase).Match(line).Success;

            if (line.Contains("OPENING BALANCE") || line.Contains("CLOSING BALANCE"))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Balance is often appended, we need to remove it
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static string StripBalance(string line)
        {
            var result = "";

            result = line;

            var pattern1 = @"\$ \$([-,0-9\.]+) DR$";

            var match = new Regex(pattern1, RegexOptions.IgnoreCase).Match(line.Trim());

            if (match.Success)
            {
                //result = decimal.Parse(match.Groups[1].Value);
                result = line.Substring(0, match.Index).Trim();
            }

            var pattern2 = @"\$ \$([-,0-9\.]+) CR$";
            var match2 = new Regex(pattern2, RegexOptions.IgnoreCase).Match(line.Trim());
            if (match2.Success)
            {
                //result = decimal.Parse(match.Groups[1].Value);
                result = line.Substring(0, match2.Index).Trim();
            }

            return result;
        }

        public static decimal? GetAmountFromLine(string line)
        {
            decimal? result = null;

            // trim balance so we can get the transaction amount
            line = StripBalance(line);

            line = TrimEndAlpha(line);

            var num = line.Substring(line.LastIndexOf(' ') + 1);

            if (num.Contains(".") && decimal.TryParse(num, out decimal number))
            {
                result = number;
            }

            // temp
            var pattern = @" \$([-,0-9\.]+)";

            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(line);

            if (match.Success)
            {
                result = decimal.Parse(match.Groups[1].Value);
            }

            if (result == null)
            {
                result = GetAmountFromLineInterest(line);
            }

            return result;
        }

        private static decimal? GetAmountFromLineInterest(string line)
        {
            decimal? result = null;

            var pattern = @"to \w+ \d{1,2}, \d{4} is ([-,0-9\.]+) ";

            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(line);

            if (match.Success)
            {
                result = decimal.Parse(match.Groups[1].Value);
            }

            return result;
        }

        //

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

            //if (line.Contains("CREDIT INTEREST"))
            //{
            //    var o = 0;
            //}

            var pattern = @" [\(|\)] \$[-,0-9\.]+ [C|D]R";// @" \( \$[-,0-9\.]+ [C|D]R";

            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(line);

            if (match.Success)
            {
                result = line.Replace(match.Value, "");
                result = result.TrimEnd();
                result = result.TrimEnd(')');
                result = result.TrimEnd('(');
            }
            else
            {
                result = TrimEndBalanceNoBracket(line);
            }
            return result;
        }

        public static string TrimEndBalanceNoBracket(string line)
        {
            string result = line;

            var pattern = @" (\$[-,0-9\.]+) \$[-,0-9\.]+ [C|D]R";

            var match = new Regex(pattern, RegexOptions.IgnoreCase).Match(line);

            if (match.Success)
            {
                if (match.Groups.Count > 1)
                {
                    result = line.Replace(match.Value, "");
                    result = result + " " + match.Groups[1].Value;
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