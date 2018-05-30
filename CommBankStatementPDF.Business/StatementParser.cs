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

        /// <summary>
        /// Read CBA PDF file and load Transactions
        /// </summary>
        /// <param name="filename"></param>
        public void ReadFile(string filename)
        {
            var fi = new FileInfo(filename);

            this.Year = Convert.ToInt32(fi.Name.Substring(9, 4));

            this.Source = IOHelper.ReadPdfFile(fi.FullName);

            GetTransactions();
        }

        /// <summary>
        /// Extract tranactions from lines of text
        /// </summary>
        /// <returns></returns>
        /// <remarks> find transactions - "Date Transaction Details Amount (A$)"</remarks>

        public string GetTransactions()
        {
            this.Transactions = new List<Transaction>();

            //var lines = this.Source.Split('\n');
            var lines = new List<string>(this.Source.Split('\n'));

            var trans = new StringBuilder();

            //bool foundTransLine = false;

            for (int i = 0; i < lines.Count - 1; i++)
            {
                Transaction newTrans = null;

                if (i < lines.Count - 2)
                {
                    newTrans = new Transaction(new List<string>() { lines[i], lines[i + 1], lines[i + 2] }, Year);
                }
                else
                {
                    newTrans = new Transaction(lines[i], Year);
                }

                if (newTrans.ParseSuccess)
                {
                    //foundTransLine = true;

                    this.Transactions.Add(newTrans);
                }
                trans.AppendLine(lines[i]);
            }

            return trans.ToString();
        }

        //public string GetTransactionsOLD()
        //{
        //    this.Transactions = new List<Transaction>();

        //    var lines = this.Source.Split('\n');

        //    var trans = new StringBuilder();

        //    bool foundTransLine = false;

        //    foreach (var item in lines)
        //    {
        //        var newTrans = new Transaction(item, Year);
        //        if (newTrans.ParseSuccess)
        //        {
        //            foundTransLine = true;

        //            this.Transactions.Add(newTrans);
        //        }
        //        trans.AppendLine(item);
        //    }

        //    return trans.ToString();
        //}
    }
}