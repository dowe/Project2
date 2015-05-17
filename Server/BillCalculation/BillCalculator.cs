using Common.DataTransferObjects;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.BillCalculation
{
    public class BillCalculator : IBillCalculator
    {
        public Bill CalculateBill(List<Test> tests)
        {
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont fontBig = new XFont("Arial", 22);
            XFont fontSmall = new XFont("Arial", 18);





            return null;

        }
    }
}
