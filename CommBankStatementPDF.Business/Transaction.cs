using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Biller { get; set; }
        public string Source { get; private set; }
        public bool ParseSuccess { get; private set; }

        private List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public Transaction()
        {
        }

        public Transaction(string line, int year) : this(new List<string>() { line }, year)
        {
        }

        public Transaction(IList<string> lines, int year)
        {
            const int MIN_LENGTH = 6;// 24;

            var line = lines[0];
            //Trace.WriteLine("LINE: " + line);

            this.Source = line;

            if (line.Length > MIN_LENGTH && IsCrap(line) == false)
            {
                if (IsCrap(line))
                {
                    Trace.WriteLine(line);
                }

                Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);
                Match m = r.Match(line.Substring(0, 6));

                if (m.Success)
                {
                    var month = m.Value.Split(' ')[1];     // had a gutful of regex now
                    var day = m.Value.Substring(0, 2);

                    var lineDate = GetDateFromLine(line, year);

                    //30/03/2015
                    if (lineDate.HasValue && lineDate.Value.Equals(new DateTime(2015, 5, 29)))
                    {
                        var o = 0;
                    }

                    if (lineDate.HasValue)
                    {
                        Trace.WriteLine(string.Format("NEW TRANS: {0}", lines[0]));

                        this.Date = lineDate.Value;

                        //this.Biller = line.Substring(7, line.LastIndexOf(' ') - 7);
                        this.Biller = line.Substring(7);

                        var multiLine = lines.Count >= 2 && lines[1].StartsWith("##");

                        var amount = GetAmountFromLine(line);
                        if (!multiLine && amount.HasValue)
                        {
                            this.Amount = amount.Value;
                        }
                        else
                        {
                            Trace.WriteLine("       " + lines[0]);
                            Trace.WriteLine("       " + lines[1]);
                            Trace.WriteLine("       " + lines[2]);
                            if (lines[1].StartsWith("##"))
                            {
                                var line2 = TrimEndAlpha(lines[2]);

                                if (decimal.TryParse(line2, out decimal x))
                                {
                                    this.Amount = decimal.Parse(line2);
                                }
                                else
                                {
                                    this.Amount = amount.Value;
                                }
                            }
                        }

                        this.ParseSuccess = !string.IsNullOrWhiteSpace(this.Biller);

                        Trace.WriteLine(this.ToString());
                        Trace.WriteLine("----------------------------------------------------------------");
                        Trace.WriteLine("");
                    }
                }
            }
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

        public static DateTime? GetDateFromLine(string value, int year)
        {
            DateTime? result = null;
            Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);
            Match m = r.Match(value.Substring(0, 6));

            var month = m.Value.Split(' ')[1];     // had a gutful of regex now
            var day = m.Value.Substring(0, 2);

            if (DateTime.TryParse("01-" + month + "-2000", out DateTime monthIndex))
            {
                int dayIndex = Convert.ToInt32(day);
                result = new DateTime(year, monthIndex.Month, dayIndex);
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

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("[" + this.GetType().FullName + "]");
            result.AppendLine("Date: " + this.Date);
            result.AppendLine("Biller: " + this.Biller);
            result.AppendLine("Amount: " + this.Amount);
            result.AppendLine("Source: " + this.Source);

            return result.ToString();
        }
    }
}