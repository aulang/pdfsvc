using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdfsvc.Core
{
    public class RegexLocationUtils
    {
        public static List<IPdfTextLocation> ExtractLocation(PdfDocument pdfDocument, StampInfo stampInfo)
        {
            List<IPdfTextLocation> locations = new List<IPdfTextLocation>();

            RegexBasedLocationExtractionStrategy strategy = new RegexBasedLocationExtractionStrategy(stampInfo.Regex);

            PdfDocumentContentParser parser = new PdfDocumentContentParser(pdfDocument);

            int totalPages = pdfDocument.GetNumberOfPages();

            foreach (int p in stampInfo.Pages)
            {
                int page = p;
                if (page < 0)
                {
                    page = totalPages + page + 1;
                }

                parser.ProcessContent(page, strategy);

                ICollection<IPdfTextLocation> collection = strategy.GetResultantLocations();

                foreach (IPdfTextLocation e in collection)
                {
                    ((DefaultPdfTextLocation)e).SetPageNr(page);
                }

                locations.AddRange(collection);
            }

            return locations;
        }
    }
}
