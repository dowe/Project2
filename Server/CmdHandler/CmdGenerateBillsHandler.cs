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
             db.StartTransaction();
             _OrderList = db.GetAllOrders(x => x.Invoiced == false);
             db.EndTransaction(TransactionEndOperation.READONLY);

             Console.WriteLine("ok1");

             //Start Creating a Mockorder for testing
             Order MockOrder1 = new Order();
             MockOrder1.CollectDate = DateTime.Now;
             MockOrder1.Invoiced = false;
             Customer MockCustomer = new Customer();
             MockCustomer.UserName = "myUsername";
             MockCustomer.LastName = "Müller";
             MockCustomer.FirstName = "Hans";
             Console.WriteLine("ok2");

           //  MockOrder.Customer.BankAccount = new BankAccount("DE 2323 1212 3333 1111", "Hans Müller");
             Console.WriteLine("ok3");

             MockCustomer.TwoWayRoadCostInEuro = 42.11f;
             MockOrder1.Customer = MockCustomer;
           
             MockOrder1.OrderID = 1111;
             List<Test> MockTest = new List<Test>();
             MockTest.Add(new Test("Patientenid123", new Analysis("Blutzeug", 1.0f, 11.0f, "Kilo", 42.42f, SampleType.BLOOD)));
             MockTest.Add(new Test("Patientenid222", new Analysis("Urinzeug", 1.0f, 11.0f, "Kilo", 35.99f, SampleType.URINE)));
             MockTest.Add(new Test("Patientenid123", new Analysis("Blutzeug", 1.0f, 11.0f, "Kilo", 42.42f, SampleType.BLOOD)));
             MockTest.Add(new Test("Patientenid999", new Analysis("Blutzeug", 1.0f, 11.0f, "Kilo", 42.42f, SampleType.BLOOD)));
             MockTest.Add(new Test("Patientenid123", new Analysis("Urinzeug", 1.0f, 11.0f, "Kilo", 35.99f, SampleType.URINE)));
             MockOrder1.Test = MockTest;
             _OrderList.Add(MockOrder1);

             //Zweite MockOrder
             Order MockOrder2 = new Order();
             MockOrder2.CollectDate = DateTime.Now;
             MockOrder2.Invoiced = false;
             Customer MockCustomer2 = new Customer();
             MockCustomer2.UserName = "otherUsername";
             MockCustomer2.LastName = "Müller";
             MockCustomer2.FirstName = "Hans";
             Console.WriteLine("ok2");

             //  MockOrder.Customer.BankAccount = new BankAccount("DE 2323 1212 3333 1111", "Hans Müller");
             Console.WriteLine("ok3");

             MockCustomer2.TwoWayRoadCostInEuro = 69.96f;
             MockOrder2.Customer = MockCustomer2;

             MockOrder2.OrderID = 2222;
             List<Test> MockTest2 = new List<Test>();
             MockTest2.Add(new Test("Patientenid123", new Analysis("Spuckezeug", 1.0f, 11.0f, "Kilo", 33.33f, SampleType.SALIVA)));
             MockTest2.Add(new Test("Patientenid222", new Analysis("Urinzeug", 1.0f, 11.0f, "Kilo", 35.99f, SampleType.URINE)));
    
             MockOrder2.Test = MockTest2;
             _OrderList.Add(MockOrder2);

             //Start MockOrder 3, same customer as 1
             Order MockOrder3 = new Order();
             MockOrder3.CollectDate = DateTime.Now;
             MockOrder3.Invoiced = false;
             Console.WriteLine("ok2");

             //  MockOrder.Customer.BankAccount = new BankAccount("DE 2323 1212 3333 1111", "Hans Müller");
             Console.WriteLine("ok3");

          
             MockOrder3.Customer = MockCustomer;

             MockOrder3.OrderID = 333;
             List<Test> MockTest3 = new List<Test>();
             MockTest3.Add(new Test("Patientenid123", new Analysis("Spermazeug", 1.0f, 11.0f, "Kilo", 33.33f, SampleType.SPERM)));
             MockTest3.Add(new Test("Patientenid222", new Analysis("Urinzeug", 1.0f, 11.0f, "Kilo", 35.99f, SampleType.URINE)));
             MockTest3.Add(new Test("Patientenid123", new Analysis("Spermazeug", 1.0f, 11.0f, "Kilo", 33.33f, SampleType.SPERM)));
             MockTest3.Add(new Test("Patientenid999", new Analysis("Blutzeug", 1.0f, 11.0f, "Kilo", 42.42f, SampleType.BLOOD)));
            
             MockOrder3.Test = MockTest3;
             _OrderList.Add(MockOrder3);
             //finish Mockorder

             Console.WriteLine("Start");
             
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

                     Console.WriteLine("Start pdf");
                     //generate PDF
                     PdfDocument doc = new PdfDocument();
                     PdfPage page = doc.AddPage();
                     page.Size = PdfSharp.PageSize.A4;
                     XGraphics gfx = XGraphics.FromPdfPage(page);
                     XTextFormatter tf = new XTextFormatter(gfx);
                     XPen pen = new XPen(XColors.Black, 1);

                     Console.WriteLine("Start headlines");


                     //Start Headlines
                     XRect headline1 = new XRect(marginLeft, y, page.Width, fontHeightBig);
             
                     gfx.DrawString("Rechnung vom " + now.ToString("dd-MM-yyyy"), fontBig, XBrushes.Black, headline1, XStringFormats.TopLeft);
                     y += fontHeightBig;
                     y += absatz;

                     XRect headline2 = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                     gfx.DrawString("Für den Zeitraum vom " + now.AddDays(-8).ToString("dd-MM-yyyy") + " bis " + now.AddDays(-1).ToString("dd-MM-yyyy"), fontSmall, XBrushes.Black, headline2, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += 2*absatz;

                     Console.WriteLine("Start orders");

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

                     Console.WriteLine("Start tests");
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
                     Console.WriteLine("Start total");
                     //Start Total
                     XRect total1 = new XRect(30, y, page.Width, fontHeightSmall);
                     XRect total2 = new XRect(marginLeft, y, page.Width , fontHeightSmall);
                     gfx.DrawString("Gesamt:", fontSmallBold, XBrushes.Black, total1, XStringFormats.Center);
                     gfx.DrawString(pricetotal.ToString() , fontSmallBold, XBrushes.Black, total2, XStringFormats.Center);
                     y += fontHeightSmall;
                     y += absatz;

                     Console.WriteLine("Start text");

                     XRect totaltext = new XRect(marginLeft, y, page.Width-2*marginLeft, fontHeightSmall*4);
                     tf.DrawString("Der Betrag in Höhe von " + pricetotal.ToString() +" wird von ihrem Konto (IBAN: TODO: BankAccount hängt"
                        // + b.Customer.BankAccount.IBAN 
                        + ") abgebucht."
                         , fontSmall, XBrushes.Black, totaltext, XStringFormats.TopLeft);

 

 


                     //Start saving PDF on server
                     Console.WriteLine("Creating Dir " + targetdir);

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
                     }
                     Console.WriteLine("Finish saving pdf");
                 }

             }

         }

    }
}
