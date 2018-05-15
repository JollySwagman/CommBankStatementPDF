﻿using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Diagnostics;
using System.Text;

namespace CommBankStatementPDF.Business
{
    public class IOHelper
    {
        public static string ReadPdfFile(string fileName)
        {
            Trace.WriteLine("READING " + fileName);

            StringBuilder text = new StringBuilder();

            PdfReader pdfReader = new PdfReader(fileName);

            for (int page = 1; page <= pdfReader.NumberOfPages; page++)
            {
                //ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                text.Append(currentText);
            }
            pdfReader.Close();

            return text.ToString();
        }
    }
}