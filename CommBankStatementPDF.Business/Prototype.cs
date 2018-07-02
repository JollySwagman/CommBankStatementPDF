using System;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class Prototype
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Line0 { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public AccountType AccountType { get; set; }
        public string SourceFile { get; set; }

        public string Source { get; set; }

        public bool IsValid()
        {
            return string.IsNullOrEmpty(this.Line0) == false;
        }

        public string Biller
        {
            get
            {
                //return (this.Line0.Substring(7) + "|").Trim() + (this.Line1 + "|").Trim() + (this.Line2 + "|").Trim() + (this.Line3 + "|").Trim();
                return (this.Line0);
            }
            private set { }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("[" + this.GetType().FullName + "]");
            result.AppendLine("Date: " + this.Date);
            result.AppendLine("AccountType: " + this.AccountType);
            result.AppendLine("SourceFile: " + this.SourceFile);
            result.AppendLine("Biller: " + this.Biller);
            result.AppendLine("Line0: " + this.Line0);
            result.AppendLine("Line1: " + this.Line1);
            result.AppendLine("Line3: " + this.Line3);
            result.Append("Amount: " + this.Amount);
            if (this.Amount == 0)
            {
                result.Append(" AMOUNT IS ZERO!");
            }
            result.AppendLine("");

            return result.ToString();
        }
    }
}