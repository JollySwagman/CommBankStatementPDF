using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

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

        public string GetTransactions()
        {
            this.Transactions = new List<Transaction>();
            //this.Year = GetYear(pdfText);

            var lines = this.Source.Split('\n');

            var trans = new StringBuilder();

            //find transactions - "Date Transaction Details Amount (A$)"
            foreach (var item in lines)
            {
                //if (item.StartsWith("Statement Period "))
                //{
                //    var sYear = item.Substring(24, 4);
                //    //Year = Convert.ToInt32(sYear);
                //}

                if (Transaction.IsTransaction(item))
                {
                    this.Transactions.Add(new Transaction(item, Year));
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

        //    public int GetYear(string pdfText)
        //    {
        //        var result = 0;

        //        //Statement Period 24 Mar 2017
        //        Regex r = new Regex(@"\d{2} [A-z]{3} \d{4}", RegexOptions.IgnoreCase);
        //        Match m = r.Match(pdfText);

        //        if (m.Success)
        //        {
        //            var sYear = m.Value.Substring(m.Value.Length - 4);
        //            result = Convert.ToInt32(sYear);

        //            Trace.WriteLine("YEAR=" + result);
        //        }

        //        return result;
        //    }
        //
    }

}