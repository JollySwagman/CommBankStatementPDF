using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class IOHelper
    {
        //public static string XXReadPdfFile(string fileName)
        //{
        //    StringBuilder text = new StringBuilder();

        //    PdfReader pdfReader = new PdfReader(fileName);

        //    for (int page = 1; page <= pdfReader.NumberOfPages; page++)
        //    {
        //        //ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
        //        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
        //        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

        //        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
        //        text.Append(currentText);
        //    }
        //    pdfReader.Close();

        //    return text.ToString();
        //}

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

        public static List<string> XXXParser2(List<string> pages)
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