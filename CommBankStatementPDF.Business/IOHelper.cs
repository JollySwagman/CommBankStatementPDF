using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class IOHelper
    {
        public static List<Prototype> GetPrototypes(string fileName)
        {
            var text = ReadPdfFileToPages(fileName);
            var lines = GetLinesFromPages(text);

            var parser = new NewParser();

            //var result = parser.GetPrototypesFromLines(new Queue<string>(lines));
            var result = parser.GetPrototypesFromLines(lines);

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
    }
}