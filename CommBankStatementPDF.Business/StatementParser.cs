using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class StatementParser
    {
        public int Year { get; private set; }

        public List<Transaction> Transactions { get; set; }

        public string GetTransactions(string pdfText)
        {
            this.Transactions = new List<Transaction>();

            var lines = pdfText.Split('\n');

            var trans = new StringBuilder();

            //find transactions - "Date Transaction Details Amount (A$)"
            foreach (var item in lines)
            {
                if (IsCrap(item))
                {
                    var o = 0;
                }

                if (item.StartsWith("Statement Period "))
                {
                    var sYear = item.Substring(24, 4);
                    Year = Convert.ToInt32(sYear);
                }

                if (IsTransaction(item))
                {
                    this.Transactions.Add(ParseTransaction(item));
                    trans.AppendLine(item);
                }
                else
                {
                    var o = 0;
                }
            }

            //Trace.WriteLine(trans.ToString());

            return trans.ToString();
        }

        /// <summary>
        /// Line begins with a date
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsTransaction(string line)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(line) && line.Length > 6)
            {
                List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                Regex r = new Regex(@"^\d{2} [A-z]{3}", RegexOptions.IgnoreCase);
                Match m = r.Match(line.Substring(0, 6));

                result = m.Success;

                if (m.Success)
                {
                    var newTran = new Transaction()
                    var month = m.Value.Substring(3);
                    result = months.Contains(month);
                }
            }

            return result;
        }

        /// <summary>
        /// Line begins with a date
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public Transaction ParseTransaction(string line)
        {
            Transaction result = null;

            if (IsTransaction(line))
            {
                List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                Regex r = new Regex(@"^\d{2} [A-z]{3}", RegexOptions.IgnoreCase);
                Match m = r.Match(line.Substring(0, 6));

                if (m.Success)
                {
                    var month = m.Value.Substring(3);
                    var xxx = months.Contains(month);

                    var biller = line.Substring(7, line.LastIndexOf(' ') - 7);

                    decimal amount = 0;
                    decimal number = 0;

                    var sAmount = line.Substring(line.LastIndexOf(' ') + 1);
                    if (decimal.TryParse(sAmount, out number))
                    {
                        amount = number;
                    }

                    int monthIndex = Convert.ToDateTime("01-" + month + "-2000").Month;

                    var dd = new DateTime(this.Year, 1, 1);

                    if (monthIndex > 6)
                    {
                    }

                    result = new Transaction { Amount = amount, Biller = biller, Date = dd };
                }
            }

            return result;
        }

        public static bool IsCrap(string line)
        {
            //e.g.  25 Apr 2017 - 24 May 2017

            var result = false;
            if (!string.IsNullOrWhiteSpace(line) && line.Length > 20)
            {
                Regex r = new Regex(@"\d{2} [A-z]{3} \d{4}", RegexOptions.IgnoreCase);
                Match m = r.Match(line);
                result = m.Success;
            }
            return result;
        }
    }
}