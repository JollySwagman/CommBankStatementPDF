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

        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendLine("[" + this.GetType().FullName + "]");
            result.AppendLine("Date: " + this.Date);
            result.AppendLine("Line0: " + this.Line0);
            result.AppendLine("Line1: " + this.Line1);
            result.AppendLine("Line3: " + this.Line3);
            result.Append("Amount: " + this.Amount);
            if (this.Amount == 0)
            {
                result.Append(" !!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            }
            result.AppendLine("");

            return result.ToString();
        }
    }
}