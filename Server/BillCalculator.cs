using Common.DataTransferObjects;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class BillCalculator : IBillCalculator
    {
        PdfDocument doc;
        String IBillCalculator.CalculateBill(List<Test> tests)
        {
            //PDF erstellen und seite adden
            doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            //Draw
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
            new XRect(0, 0, page.Width, page.Height),
            XStringFormats.Center);

            //Abspeichern
            string filename = "HelloWorld.pdf";
            doc.Save(filename);

            return "";
        }

       
    }
}
