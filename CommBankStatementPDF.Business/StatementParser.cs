using CommBankStatementPDF.Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OLD.CommBankStatementPDF.Business
{

    /// <summary>
    /// Manage the process of converting PDF to a collection of Transactions
    /// </summary>
    public class StatementParser
    {

        public int Year { get; private set; }
        public List<Transaction> Transactions { get; set; }
        public List<string> Lines { get; set; }
        public string Source { get; set; }
        public FileInfo Filename { get; set; }
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
            this.Filename = fi;
            this.Year = Convert.ToInt32(fi.Name.Substring(9, 4));
            this.Lines = IOHelper.ReadPdfFileToPages(fi.FullName);
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

            var lines = this.Lines; //new List<string>(this.Source.Split('\n'));

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Transaction newTrans = null;

                if (lines[i].Contains("373578"))
                {
                    var t = 0;
                }

                if (i < lines.Count - 2)
                {
                    //newTrans = new Transaction(new List<string>() { lines[i], lines[i + 1], lines[i + 2] }, Year, this.Type);
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

                    newTrans.SourceFile = this.Filename.Name;

                    this.Transactions.Add(newTrans);
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("[" + this.GetType().FullName + "]");
            result.AppendLine("Type: " + this.Type.ToString());
            result.AppendLine("Year: " + this.Year);
            result.AppendLine("Year: " + this.Filename.FullName);
            result.AppendLine("Transactions: " + this.Transactions.Count);

            return result.ToString();
        }
    }
}

