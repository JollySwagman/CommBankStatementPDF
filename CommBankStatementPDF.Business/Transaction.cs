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

            //Trace.Write("PARSING: " + line);

            if (line.Length > 10) //(!IsCrap(line) && Transaction.IsTransaction(line))
            {
                List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);
                Match m = r.Match(line.Substring(0, 6));

                if (m.Success)
                {
                    var month = m.Value.Split(' ')[1];     // had a gutful of regex now
                    var day = m.Value.Substring(0, 2);

                    if (months.Contains(month))
                    {
                        var biller = "";
                        if (line.Length > 20)
                        {
                            biller = line.Substring(7, line.LastIndexOf(' ') - 7);
                        }

                        if (DateTime.TryParse("01-" + month + "-2000", out DateTime monthIndex))
                        {
                            int dayIndex = Convert.ToInt32(day);
                            var transactionDate = new DateTime(year, monthIndex.Month, dayIndex);

                            
                            if (decimal.TryParse(line.Substring(line.LastIndexOf(' ') + 1), out decimal number))
                            {
                                this.Amount = number;
                            }

                            this.Biller = biller;
                            this.Date = transactionDate;

                            this.ParseSuccess = !string.IsNullOrWhiteSpace(biller);

                            //Trace.WriteLine(" SUCCESS: " + this.ParseSuccess);
                        }
                    }
                }
            }
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