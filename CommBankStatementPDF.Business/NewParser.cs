using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommBankStatementPDF.Business
{
    public class NewParser
    {
        public List<Prototype> GetPrototypesFromLines(List<string> lines, int year, AccountType accountType, string filename)
        {
            var result = new List<Prototype>();
            var newProto = new Prototype();
            var lineCount = 0;

            foreach (var line in lines)
            {
                Trace.WriteLine("   >>> " + line);

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

                    var amt = LineParser.GetAmountFromLine(item);
                    if (amt.HasValue)
                    {
                        newProto.Amount = amt.Value;
                    }
                }
                else
                {
                    lineCount++;
                    if (lineCount == 0)
                    {
                        newProto.Line0 = item;
                        var amt = LineParser.GetAmountFromLine(item);
                        if (amt.HasValue)
                        {
                            newProto.Amount = amt.Value;
                        }
                    }
                    if (lineCount == 1)
                    {
                        newProto.Line1 = item;
                        var amt = LineParser.GetAmountFromLine(item);
                        if (amt.HasValue)
                        {
                            newProto.Amount = amt.Value;
                        }
                    }
                    if (lineCount == 2)
                    {
                        newProto.Line2 = item;
                        var amt = LineParser.GetAmountFromLine(item);
                        if (amt.HasValue)
                        {
                            newProto.Amount = amt.Value;
                        }
                    }
                    if (lineCount == 3)
                    {
                        newProto.Line3 = item;
                        var amt = LineParser.GetAmountFromLine(item);
                        if (amt.HasValue)
                        {
                            newProto.Amount = amt.Value;
                        }
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