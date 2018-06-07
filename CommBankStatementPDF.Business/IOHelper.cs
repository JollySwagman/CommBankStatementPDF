using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class IOHelper
    {
        public static List<Prototype> GetPrototypes(string fileName)
        {
            var text = ReadPdfFileToPages(fileName);
            var lines = GetLinesFromPages(text);
            var result = IOHelper.GetPrototypesFromLines(lines);

            return result;
        }

        public static List<string> ReadPdfFileToPages(string fileName)
        {
            var result = new List<string>();

            PdfReader pdfReader = new PdfReader(fileName);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));

                foreach (var item in currentText.Split('\n'))
                {
                    result.Add(item);
                }
            }
            pdfReader.Close();

            return result;
        }

        public static List<string> GetLinesFromPages(List<string> pages)
        {
            var result = new List<string>();
            var sb = new StringBuilder();

            var foundBeginning = false;

            foreach (var line in pages)
            {
                if (line.Contains(HelperVISA.BEGIN_TRANSACTIONS))
                {
                    foundBeginning = true;
                }

                if (line.Contains(HelperVISA.END_TRANSACTIONS))
                {
                    break;
                }

                if ((foundBeginning) && Transaction.IsCrap(line) == false)
                {
                    sb.AppendLine(line);
                    result.Add(line);
                }
            }

            sb.AppendLine("---------------------------------------");

            return result;
        }

        public static List<Prototype> GetPrototypesFromLines(List<string> lines)
        {
            var result = new List<Prototype>();
            var newProto = new Prototype();
            var lineCount = 0;

            foreach (var line in lines)
            {
                Trace.WriteLine("   >>> "+ line);
                var item = LineParser.TrimEndBalance(line);

                var date = Transaction.GetDateFromLine(item, 2000);
                if (date.HasValue)
                {
                    // save the previous and start a new one
                    if (newProto != null)
                    {
                        result.Add(newProto);
                    }
                    lineCount = 0;
                    newProto = new Prototype() { Date = date.Value, Line0 = item };
                }
                else
                {
                    lineCount++;
                    if (lineCount == 0)
                    {
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
                }
            }
            return result;
        }
    }
}