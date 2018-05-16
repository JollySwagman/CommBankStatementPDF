using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class StatementParser
    {
        public int Year { get; private set; }

        public List<Transaction> Transactions { get; set; }
        public string Source { get; private set; }

        public void ReadFile(string filename)
        {
            var fi = new FileInfo(filename);

            this.Year = Convert.ToInt32(fi.Name.Substring(9, 4));

            this.Source = IOHelper.ReadPdfFile(fi.FullName);

            GetTransactions();
        }

        //find transactions - "Date Transaction Details Amount (A$)"
        public string GetTransactions()
        {
            this.Transactions = new List<Transaction>();

            var lines = this.Source.Split('\n');

            var trans = new StringBuilder();

            foreach (var item in lines)
            {
                if (true)//(Transaction.IsTransaction(item))
                {
                    var newTrans = new Transaction(item, Year);
                    if (newTrans.ParseSuccess)
                    {
                        this.Transactions.Add(newTrans);
                    }
                    trans.AppendLine(item);
                }
                else
                {
                    var o = 0;
                }
            }

            return trans.ToString();
        }
    }
}