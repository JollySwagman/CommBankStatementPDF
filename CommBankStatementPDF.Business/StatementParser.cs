using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class StatementParser
    {
        public enum AccountType
        {
            VISA,
            StreamLine
        }

        public int Year { get; private set; }

        public List<Transaction> Transactions { get; set; }
        public List<string> Lines { get; set; }
        public string Source { get; set; }

        public string Filename { get; set; }

        public string FilteredSource { get; set; }

        public AccountType Type { get; private set; }

        public StatementParser(AccountType accountType)
        {
            this.Type = accountType;
        }

        public StatementParser(string filename, AccountType accountType) : this(accountType)
        {
            var fi = new FileInfo(filename);
            Trace.WriteLine("FILE: " + fi.FullName);

            //this.Type=
            this.Filename = fi.FullName;
            this.Year = Convert.ToInt32(fi.Name.Substring(9, 4));
            this.Source = IOHelper.ReadPdfFile(fi.FullName);
        }

        public void Parser2(List<string> pages)
        {
            this.Transactions = new List<Transaction>();
            this.Lines = new List<string>();
            var sb = new StringBuilder();

            for (int i = 0; i < pages.Count; i++)
            {
                var foundBeginning = false;

                var lines = pages[i].Split('\n');

                sb.AppendLine("PAGE " + i + " =================================");

                foreach (var line in lines)
                {
                    if (line.Contains(HelperVISA.BEGIN_TRANSACTIONS))
                    {
                        foundBeginning = true;
                    }

                    if (line.Contains(HelperVISA.END_TRANSACTIONS))
                    {
                        break;
                    }

                    if ((foundBeginning || i > 0) && Transaction.IsCrap(line) == false)
                    {
                        sb.AppendLine(line);
                        this.Lines.Add(line);
                    }
                }

                sb.AppendLine("---------------------------------------");
            }

            FilteredSource = sb.ToString();
        }

        public decimal GetTransactionTotal()
        {
            decimal total = 0;
            foreach (var item in this.Transactions)
            {
                total += item.Amount;
            }
            return total; // this.Transactions.Sum(x => x.Amount);
        }

        /// <summary>
        /// Read CBA PDF file and load Transactions
        /// </summary>
        /// <param name="filename"></param>
        public void ReadFile()
        {
            this.Transactions = new List<Transaction>();

            var lines = new List<string>(this.Source.Split('\n'));

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Transaction newTrans = null;

                if (lines[i].Contains("373578"))
                {
                    var t = 0;
                }

                if (i < lines.Count - 2)
                {
                    newTrans = new Transaction(new List<string>() { lines[i], lines[i + 1], lines[i + 2] }, Year, this.Type);
                }
                else
                {
                    newTrans = new Transaction(lines[i], Year, this.Type);
                }

                if (newTrans.ParseSuccess)
                {
                    if (newTrans.Amount > 3000)
                    {
                        var t = 0;
                    }

                    this.Transactions.Add(newTrans);
                }
            }
        }
    }
}