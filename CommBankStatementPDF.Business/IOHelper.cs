using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class IOHelper
    {
        public static int GetYear(string filename)
        {
            var fi = new FileInfo(filename);
            return Convert.ToInt32(fi.Name.Substring(9, 4));
        }

        /// <summary>
        /// Infer account type from folder - will do for now..
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static AccountType GetAccountType(string filename)
        {
            var fi = new FileInfo(filename);

            var folder = fi.Directory.Name.ToUpper();

            var result = AccountType.Unknown;
            switch (folder)
            {
                case "STREAMLINE":
                    result = AccountType.StreamLine;
                    break;

                case "VISA":
                    result = AccountType.VISA;
                    break;

                default:
                    break;
            }

            return result;
        }

        public int Year { get; set; }
        public AccountType AccountType { get; set; }

        public List<Prototype> GetPrototypes(string filename)
        {
            var pages = ReadPdfFileToPages(filename);
            var lines = GetLinesFromPages(pages);

            var parser = new NewParser();

            var year = GetYear(filename);
            var accountType = GetAccountType(filename);

            //var result = parser.GetPrototypesFromLines(new Queue<string>(lines));
            var result = parser.GetPrototypesFromLines(lines, year, accountType, filename);

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
            return GetLinesFromPages(pages, false);
        }

        public static List<string> GetLinesFromPages(List<string> pages, bool processAll)
        {
            var result = new List<string>();
            var sb = new StringBuilder();

            var foundBeginning = processAll;

            foreach (var line in pages)
            {
                if (line.Contains(HelperVISA.BEGIN_TRANSACTIONS) || line.Contains(HelperStreamline.BEGIN_TRANSACTIONS))
                {
                    foundBeginning = true;
                }

                if (line.Contains(HelperVISA.END_TRANSACTIONS))
                {
                    break;
                }

                if ((foundBeginning) && LineParser.IsCrap(line) == false)
                {
                    if (StreamLine.IsBracketLine(line))
                    {
                    }
                    sb.AppendLine(line);
                    result.Add(line);
                }
            }

            sb.AppendLine("---------------------------------------");

            return result;
        }
    }
}