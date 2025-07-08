using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System.Diagnostics;

namespace PDFProcessor
{
    internal class PDFOperations : IDisposable
    {
        private readonly string inputFilePath;
        private readonly string outputFilePath;
        private readonly PdfDocument pdfDocument;

        public PDFOperations(string inputFilePath, string outputFilePath)
        {
            this.inputFilePath = inputFilePath;
            this.outputFilePath = outputFilePath;
            // input file exist check 
            pdfDocument = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Modify);
        }

        internal void Watermark(string watermarkText)
        {
            var font = new XFont("Arial", 50, XFontStyle.BoldItalic);
            foreach (var page in pdfDocument.Pages)
            {
                var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
                var size = gfx.MeasureString(watermarkText, font);
                gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                gfx.RotateTransform(-45);
                gfx.DrawString(watermarkText,
                    font,
                    new XSolidBrush(XColor.FromArgb(128, 200, 200, 200)),
                    -size.Width / 2,
                    -size.Height / 2);

                gfx.Dispose();
            }
        }

        internal void GuidStamp(string guid)
        {
            var font = new XFont("Arial", 12, XFontStyle.Regular);
            foreach (var page in pdfDocument.Pages)
            {
                var gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Append);
                gfx.DrawString(guid, font, XBrushes.Black, new XPoint(20, 20));
                gfx.Dispose();
            }
        }

        internal void Save()
        {
            string parentDir = new FileInfo(outputFilePath)?.DirectoryName;
            if (!Directory.Exists(parentDir))
            {
                Directory.CreateDirectory(parentDir);
            }
            pdfDocument.Save(outputFilePath);
        }

        public void Dispose()
        {
            pdfDocument?.Dispose();
        }
    }
}