using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommBankStatementPDF.Business
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Biller { get; set; }
        public string SourceFile { get; set; }
        public IList<string> Source { get; private set; }
        public bool ParseSuccess { get; private set; }
        public StatementParser.AccountType Type { get; private set; }

        private List<string> months = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public Transaction(string line, int year, StatementParser.AccountType accountType) : this(new List<string>() { line }, year, accountType)
        {
            this.Type = accountType;
        }

        public Transaction(IList<string> lines, int year, StatementParser.AccountType accountType)
        {
            const int MIN_LENGTH = 6;// 24;

            this.Type = accountType;

            var line = lines[0];
            this.Source = lines;

            var debug = false;

            Trace.WriteLineIf(debug, "LINE0: " + lines[0]);
            if (lines.Count > 1)
            {
                Trace.WriteLineIf(debug, "LINE1: " + lines[1]);
                if (lines.Count > 2)
                {
                    Trace.WriteLineIf(debug, "LINE2: " + lines[2]);
                }
            }

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

                        if (StreamLine.IsBracketLine(line))
                        {
                            var d = 0;
                            var line2 = line.Substring(0, line.IndexOf(')'));
                            this.Amount = LineParser.GetAmountFromLine(line2).GetValueOrDefault();
                            this.Biller = line;
                        }
                        else
                        {
                            //this.Biller = line.Substring(7, line.LastIndexOf(' ') - 7);
                            this.Biller = line.Substring(7);

                            var multiLine1 = lines.Count >= 2 && lines[1].StartsWith("##");
                            var multiLine2 = lines.Count >= 2 && lines[1].Count(f => f == ')') == 1;

                            if (multiLine2)
                            {
                                var line2 = lines[1].Substring(0, lines[1].IndexOf(')'));
                                this.Amount = LineParser.GetAmountFromLine(line2).GetValueOrDefault();
                            }
                            else
                            {
                                var amount = LineParser.GetAmountFromLine(line);
                                if (!multiLine1 && amount.HasValue)
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
                                        var line2 = LineParser.TrimEndAlpha(lines[2]);

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

        public static DateTime? GetDateFromLine(string value, int year)
        {
            DateTime? result = null;
            Regex r = new Regex(@"^\d{1,2} [A-z]{3}", RegexOptions.IgnoreCase);
            Match m = r.Match(value.Substring(0, 6));

            if (!string.IsNullOrEmpty( m.Value))
            {
                var month = m.Value.Split(' ')[1];     // had a gutful of regex now
                var day = m.Value.Substring(0, 2);

                if (DateTime.TryParse("01-" + month + "-2000", out DateTime monthIndex))
                {
                    int dayIndex = Convert.ToInt32(day);
                    result = new DateTime(year, monthIndex.Month, dayIndex);
                }
            }
            return result;
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("[" + this.GetType().FullName + "]");
            result.AppendLine("Type: " + this.Type.ToString());
            result.AppendLine("Date: " + this.Date);
            result.AppendLine("Biller: " + this.Biller);
            result.AppendLine("Amount: " + this.Amount);
            foreach (var item in this.Source)
            {
                result.AppendLine("Source: " + item);
            }

            return result.ToString();
        }
    }
}