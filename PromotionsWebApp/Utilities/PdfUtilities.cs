using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PromotionsWebApp.Utilities
{
    public static class PdfUtilities
    {
        public static async Task<byte[]> CombinePDF(List<byte[]> files)
        {
            byte[] res;

            using (var outPdf = new PdfDocument())
            {
                foreach (var pdf in files)
                {
                    using (var pdfStream = new MemoryStream(pdf))
                    using (var pdfDoc = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import))
                        for (var i = 0; i < pdfDoc.PageCount; i++)
                            outPdf.AddPage(pdfDoc.Pages[i]);
                }

                using (var memoryStreamOut = new MemoryStream())
                {
                    outPdf.Save(memoryStreamOut, false);

                    res = memoryStreamOut.ToArray();
                }
            }

            return res;
        }
    }
}
