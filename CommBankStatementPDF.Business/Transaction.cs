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
        public decimal? Amount { get; set; }
        public string Biller { get; set; }
        public string Source { get; private set; }
        public bool ParseSuccess { get; private set; }

        private List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public Transaction()
        {
        }

        public Transaction(string line, int year)
        {
            this.Source = line;

            if (line.Length > 10)
            {
                Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);
                Match m = r.Match(line.Substring(0, 6));

                if (m.Success)
                {
                    var month = m.Value.Split(' ')[1];     // had a gutful of regex now
                    var day = m.Value.Substring(0, 2);

                    var lineDate = GetDateFromLine(line, year);

                    if (lineDate.HasValue)
                    {
                        this.Date = lineDate.Value;

                        if (lineDate.Value.Equals(new DateTime(2016, 2, 14)))
                        {
                            var o = 0;
                        }

                        if (line.Length > 20)
                        {
                            this.Biller = line.Substring(7, line.LastIndexOf(' ') - 7);
                        }

                        var amount = GetAmountFromLine(line);
                        if (amount.HasValue)
                        {
                            this.Amount = amount.Value;
                        }
                        else
                        {
                            Trace.WriteLine("MISSING AMOUNT! " + line);
                        }

                        this.ParseSuccess = !string.IsNullOrWhiteSpace(this.Biller);
                    }
                }
            }
        }

        public static decimal? GetAmountFromLine(string line)
        {
            decimal? result = null;

            TrimEndAlpha(line);

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
                    Trace.WriteLine(value[i]);
                    if (IsNumeric(Convert.ToString(value[i])))
                    {
                        break;
                    }
                    else
                    {
                        result = value.Substring(0, value.Length - i);
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