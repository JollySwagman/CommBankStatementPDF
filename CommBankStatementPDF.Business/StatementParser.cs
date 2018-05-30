﻿using System;
using System.Collections.Generic;
using System.IO;

namespace CommBankStatementPDF.Business
{
    public class StatementParser
    {
        public int Year { get; private set; }

        public List<Transaction> Transactions { get; set; }
        public string Source { get; private set; }

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

        public void GetTransactions()
        {
            this.Transactions = new List<Transaction>();

            var lines = new List<string>(this.Source.Split('\n'));

            //var trans = new StringBuilder();

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
                    this.Transactions.Add(newTrans);
                }
                //trans.AppendLine(lines[i]);
            }

            //return trans.ToString();
        }
    }
}