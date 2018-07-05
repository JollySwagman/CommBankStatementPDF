using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommBankStatementPDF.Business
{
    public class NewParser
    {
        public bool Verbose { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="year"></param>
        /// <param name="accountType"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<Prototype> GetPrototypesFromLines(List<string> lines, int year, AccountType accountType, string filename)
        {
            var result = new List<Prototype>();
            var newProto = new Prototype();
            var lineCount = 0;

            foreach (var line in lines)
            {
                Trace.WriteLineIf(this.Verbose, "   >>> " + line);

                var item = LineParser.TrimEndBalance(line);

                var date = LineParser.GetDateFromLine(item, year);
                if (date.HasValue)
                {
                    // save the previous and start a new one
                    if (newProto != null && string.IsNullOrWhiteSpace(newProto.Line0) == false)
                    {
                        result.Add(newProto);
                    }
                    lineCount = 0;
                    newProto = new Prototype() { AccountType = accountType, Date = date.Value, Line0 = item, SourceFile = filename };
                }
                else
                {
                    // Add subsequent lines that belong to this transaction
                    lineCount++;
                    if (lineCount == 0)
                    {
                        // should never end up here..
                        newProto.Line0 = item;
                    }
                    if (lineCount == 1)
                    {
                        newProto.Line1 = item;
                    }
                    if (lineCount == 2)
                    {
                        newProto.Line2 = item;
                    }
                    if (lineCount == 3)
                    {
                        newProto.Line3 = item;
                    }
                }
            }

            // HACK
            if (result.Count == 0 && newProto.IsValid())
            {
                result.Add(newProto);
            }

            return result;
        }
    }
}