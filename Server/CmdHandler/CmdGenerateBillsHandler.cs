using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PdfSharp.Drawing.Layout;
namespace Server.CmdHandler
{
    public class CmdGenerateBillsHandler : CommandHandler<CmdGenerateBills>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;
        List<Order> _OrderList;
        List<Test> _TestList;
         public CmdGenerateBillsHandler(
            IServerConnection connection,
            IDatabaseCommunicator db
           )
        {
            this.connection = connection;
            this.db = db;
        }

         protected override void Handle(CmdGenerateBills command, string connectionIdOrNull)
         {
             _OrderList = new List<Order>();
             db.StartTransaction();
             _OrderList = db.GetAllOrders(x => x.Invoiced == false);
             

            

             
             DateTime now = DateTime.Now;
             Bill b;
             float pricetotal = 0;
             int OrderAmount, TestAmount = 0; ;
             for (int i = 0; i < _OrderList.Count; i++)
             {

                 //New Customer whose Oders havent been invoiced
                 if (_OrderList[i].Invoiced == false)
                 {

                     _TestList = new List<Test>();
                     OrderAmount = 1;
                     TestAmount = 0;

                     //Create new Bill
                     b = new Bill();

                     b.Customer = _OrderList[i].Customer;

                     b.Date = now;

                     string targetdir = Directory.GetCurrentDirectory() + "/App_Data/" + _OrderList[i].Customer.UserName;
                     b.PDFPath = Directory.GetCurrentDirectory() + "/App_Data/" + _OrderList[i].Customer.UserName + "/" + now.ToString("dd-MM-yyyy")+".pdf";
                     //Add Testlist from Order

                     _TestList = (List<Test>) _OrderList[i].Test;

                     //Check if Customer has other Orders pending
                     for (int j = i+1; j < _OrderList.Count; j++)
                     {

                         //Yes? get all tests in customers testlist 
                         if (_OrderList[j].Customer.UserName.Equals(b.Customer.UserName) && _OrderList[j].Invoiced == false)
                         {
                             OrderAmount++;
                             _OrderList[j].Invoiced = true;
                             _TestList = _TestList.Concat(_OrderList[j].Test).ToList();
                             
                         }
                     }
                     //now all Tests and the amount of Orders should be gathered correctly for the Customer

                     
                     //PDF Styles
                     double y = 60;
                     double absatz = 15;
                     double marginLeft = 100;

                     //Fonts and Fontheights
                     double fontHeightSmall = 14;
                     double fontHeightBig = 18;
                     XFont fontSmallBold = new XFont("Arial", fontHeightSmall, XFontStyle.Bold);
                     XFont fontBig = new XFont("Arial", fontHeightBig);
                     XFont fontSmall = new XFont("Arial", fontHeightSmall);

                     //generate PDF
                     PdfDocument doc = new PdfDocument();
                     PdfPage page = doc.AddPage();
                     page.Size = PdfSharp.PageSize.A4;
                     XGraphics gfx = XGraphics.FromPdfPage(page);
                     XTextFormatter tf = new XTextFormatter(gfx);
                     XPen pen = new XPen(XColors.Black, 1);



                     //Start Headlines
                     XRect headline1 = new XRect(marginLeft, y, page.Width, fontHeightBig);
             
                     gfx.DrawString("Rechnung vom " + now.ToString("dd-MM-yyyy"), fontBig, XBrushes.Black, headline1, XStringFormats.TopLeft);
                     y += fontHeightBig;
                     y += absatz;

                     XRect headline2 = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                     gfx.DrawString("Für den Zeitraum vom " + now.AddDays(-8).ToString("dd-MM-yyyy") + " bis " + now.AddDays(-1).ToString("dd-MM-yyyy"), fontSmall, XBrushes.Black, headline2, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += 2*absatz;


                     //Start Orders
                     XRect ordershead = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                     gfx.DrawRectangle(pen, marginLeft-5, y-5, page.Width - (2*marginLeft), fontHeightSmall * 2 + absatz+10);
 
                     gfx.DrawString("Bestellungen", fontSmallBold, XBrushes.Black, ordershead, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += absatz;

                     pricetotal = OrderAmount * b.Customer.TwoWayRoadCostInEuro;

                     XRect orders = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                     gfx.DrawString(OrderAmount.ToString() + " Fahrt(en)", fontSmall, XBrushes.Black, orders, XStringFormats.TopLeft);
                     gfx.DrawString((b.Customer.TwoWayRoadCostInEuro * OrderAmount).ToString(),fontSmall, XBrushes.Black, orders, XStringFormats.Center);
                     y += fontHeightSmall;
                     y += 2*absatz;

                     //Start Tests
                     XRect testshead = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                    
                     gfx.DrawString("Abgeschlossene Tests", fontSmallBold, XBrushes.Black, testshead, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += absatz;
                     
                     foreach (Test t in _TestList)
                     {
                         pricetotal += t.Analysis.PriceInEuro;
                         XRect tests = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                         gfx.DrawString(t.Analysis.Name, fontSmall, XBrushes.Black, tests, XStringFormats.TopLeft);
                         gfx.DrawString(t.Analysis.PriceInEuro.ToString(), fontSmall, XBrushes.Black, tests, XStringFormats.Center);
                         y += fontHeightSmall;
                         TestAmount++;
                     }
                 
                     gfx.DrawRectangle(pen, marginLeft - 5, y - 5 - ((TestAmount +1) * fontHeightSmall) - absatz  , page.Width - (2 * marginLeft),  ((TestAmount+1)*fontHeightSmall)+absatz+ 10);

                     y += 2*absatz;
                     //Start Total
                     XRect total1 = new XRect(30, y, page.Width, fontHeightSmall);
                     XRect total2 = new XRect(marginLeft, y, page.Width , fontHeightSmall);
                     gfx.DrawString("Gesamt:", fontSmallBold, XBrushes.Black, total1, XStringFormats.Center);
                     gfx.DrawString(pricetotal.ToString() , fontSmallBold, XBrushes.Black, total2, XStringFormats.Center);
                     y += fontHeightSmall;
                     y += absatz;


                     XRect totaltext = new XRect(marginLeft, y, page.Width-2*marginLeft, fontHeightSmall*4);
                     tf.DrawString("Der Betrag in Höhe von " + pricetotal.ToString() +" wird von ihrem Konto (IBAN: "
                         + b.Customer.BankAccount.IBAN 
                        + ") abgebucht."
                         , fontSmall, XBrushes.Black, totaltext, XStringFormats.TopLeft);

 

 


                     //Start saving PDF on server

                     //Create Directory For Customer
                     if (Directory.Exists(targetdir))
                     {
                         Console.WriteLine("Dir not created");
                     }
                     else
                     {
                         Console.WriteLine("Dir created");
                         Directory.CreateDirectory(targetdir);
                     }

                     //Check if PDF File already Exists 
                     //TODO: Throw Exception
                     if (File.Exists(b.PDFPath))
                     {
                         Console.WriteLine("Error, File already Exists at: " + b.PDFPath);
                     }
                     else
                     {
                         doc.Save(b.PDFPath);
                         db.CreateBill(b);
                     }
                 }

             }
         db.EndTransaction(TransactionEndOperation.SAVE);

         }

    }
}
