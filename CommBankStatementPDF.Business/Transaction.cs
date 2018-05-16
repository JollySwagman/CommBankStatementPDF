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

        public Transaction()
        {
        }

        public Transaction(string line, int year)
        {
            this.Source = line;

            Trace.Write("PARSING: " + line);

            if (line.Length > 10) //(!IsCrap(line) && Transaction.IsTransaction(line))
            {
                List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);
                Match m = r.Match(line.Substring(0, 6));

                if (m.Success)
                {
                    var month = m.Value.Split(' ')[1];     // had a gutful of regex now
                    //var month = m.Value.Substring(3);
                    var day = m.Value.Substring(0, 2);

                    if (months.Contains(month))
                    {
                        var biller = "";
                        if (line.Length > 20)
                        {
                            biller = line.Substring(7, line.LastIndexOf(' ') - 7);
                        }

                        decimal amount = 0;

                        var sAmount = line.Substring(line.LastIndexOf(' ') + 1);
                        if (decimal.TryParse(sAmount, out decimal number))
                        {
                            amount = number;
                        }

                        int dayIndex = Convert.ToInt32(day);

                        DateTime monthIndex;// Convert.ToDateTime("01-" + month + "-2000").Month;

                        if (DateTime.TryParse("01-" + month + "-2000", out monthIndex))
                        {
                            var dd = new DateTime(year, monthIndex.Month, dayIndex);

                            this.Amount = amount;
                            this.Biller = biller;
                            this.Date = dd;

                            this.ParseSuccess = !string.IsNullOrWhiteSpace(biller);

                            Trace.WriteLine(" SUCCESS: " + this.ParseSuccess);
                        }

                    }
                }
            }
        }

        ///// <summary>
        ///// Line begins with a date
        ///// </summary>
        ///// <param name="line"></param>
        ///// <returns></returns>
        ///// <remarks>Looking for lines with "07 Jan " or "7 Jan" with valid month names</remarks>
        //public static bool IsTransaction(string line)
        //{
        //    var result = false;

        //    if (!IsCrap(line) && !string.IsNullOrWhiteSpace(line) && line.Length > 6)
        //    {
        //        List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        //        var pattern = @"^\d{2} [A-z]{3} ";
        //        pattern = @"^\d{1,2} [A-z]{3} ";

        //        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        //        Match m = r.Match(line.Substring(0, 6));

        //        result = m.Success;

        //        if (m.Success)
        //        {
        //            var mm = m.Value.Split(' ')[1];     // had a gutful of regex now
        //            result = months.Contains(mm);
        //        }
        //    }

        //    return result;
        //}

        //public static bool IsCrap(string line)
        //{
        //    //e.g.  25 Apr 2017 - 24 May 2017

        //    var result = false;
        //    //if (!string.IsNullOrWhiteSpace(line) && line.Length > 20)
        //    //{
        //    //    Regex r = new Regex(@"\d{1,2} [A-z]{3} \d{4}", RegexOptions.IgnoreCase);
        //    //    Match m = r.Match(line);
        //    //    result = m.Success;
        //    //}

        //    //Trace.WriteLine(string.Format("IsCrap {0} ({1})", result, line));

        //    return result;
        //}

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