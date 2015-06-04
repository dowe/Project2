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
        Dictionary<String, AmountPricePair> dic;
         public CmdGenerateBillsHandler(
            IServerConnection connection,
            IDatabaseCommunicator db
           )
        {
            this.connection = connection;
            this.db = db;
        }

        class AmountPricePair
        {
            int _Amount;
            float _Price;
            public AmountPricePair(int Amount, float Price)
            {
                this._Amount = Amount;
                this._Price = Price;
            }
            public float Price
            {
                get { return _Price; }
                set { _Price = value; }
            }

            public int Amount
            {
                get { return _Amount; }
                set { _Amount = value; }
            }
           
          
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
                     String mainDirectory = Directory.GetParent(Directory.GetParent(Directory.GetParent( Directory.GetCurrentDirectory().ToString()).ToString()).ToString()).ToString();
                     String targetdir = mainDirectory + "/ASPServer/App_Data/" + _OrderList[i].Customer.UserName;
                     b.PDFPath = mainDirectory + "/ASPServer/App_Data/" + _OrderList[i].Customer.UserName + "/" + now.ToString("dd-MM-yyyy")+".pdf";
                     
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
                     
                     //Create a Dictionary with Name as Key and Amount as value
                     dic = new Dictionary<String,AmountPricePair>();
                     foreach(Test t in _TestList)
                     {
                         if (dic.ContainsKey(t.Analysis.Name))
                         {
                             dic[t.Analysis.Name] = new AmountPricePair(dic[t.Analysis.Name].Amount+1, dic[t.Analysis.Name].Price+ t.Analysis.PriceInEuro);
                         }
                         else
                             dic.Add(t.Analysis.Name, new AmountPricePair(1 , t.Analysis.PriceInEuro));
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

                     XRect amountFahrt = new XRect(marginLeft, y, page.Width / 2, fontHeightSmall);
                     gfx.DrawString(OrderAmount.ToString(), fontSmall, XBrushes.Black, amountFahrt, XStringFormats.TopLeft);

                     XRect orders = new XRect(marginLeft + 20, y, page.Width / 2, fontHeightSmall);
                     gfx.DrawString("x Fahrt(en)", fontSmall, XBrushes.Black, orders, XStringFormats.TopLeft);
                     tf.Alignment = XParagraphAlignment.Right;
                     tf.DrawString((b.Customer.TwoWayRoadCostInEuro * OrderAmount).ToString("C"),fontSmall, XBrushes.Black, orders, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += 2*absatz;

                     //Start Tests
                     XRect testshead = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                    
                     gfx.DrawString("Abgeschlossene Tests", fontSmallBold, XBrushes.Black, testshead, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += absatz;
                     
                     //Loop through all Tests in Dictionary and print them

                     foreach(var entry in dic)
                     {
                         pricetotal += entry.Value.Price;
                         XRect amount = new XRect(marginLeft, y, page.Width / 2, fontHeightSmall);
                         gfx.DrawString(entry.Value.Amount.ToString(), fontSmall, XBrushes.Black, amount, XStringFormats.TopLeft);
                         XRect tests = new XRect(marginLeft + 20, y, page.Width / 2, fontHeightSmall);
                         gfx.DrawString("x "+entry.Key, fontSmall, XBrushes.Black, tests, XStringFormats.TopLeft);
                         tf.DrawString(entry.Value.Price.ToString("C"), fontSmall, XBrushes.Black, tests, XStringFormats.TopLeft);
                         y += fontHeightSmall;
                         TestAmount++;

                     }
                     gfx.DrawRectangle(pen, marginLeft - 5, y - 5 - ((TestAmount +1) * fontHeightSmall) - absatz  , page.Width - (2 * marginLeft),  ((TestAmount+1)*fontHeightSmall)+absatz+ 10);

                     y += 2*absatz;
                     //Start Total
                     XRect total1 = new XRect(15, y, page.Width, fontHeightSmall);
                     XRect total2 = new XRect(marginLeft+17, y, page.Width/2 , fontHeightSmall);
                     gfx.DrawString("Gesamt:", fontSmallBold, XBrushes.Black, total1, XStringFormats.Center);
                     tf.DrawString(pricetotal.ToString("C") , fontSmallBold, XBrushes.Black, total2, XStringFormats.TopLeft);
                     y += fontHeightSmall;
                     y += absatz;

                     tf.Alignment = XParagraphAlignment.Left;
                     XRect totaltext = new XRect(marginLeft, y, page.Width-2*marginLeft, fontHeightSmall*4);
                     tf.DrawString("Der Betrag in Höhe von " + pricetotal.ToString("C") +" wird von ihrem Konto (IBAN: "
                         + b.Customer.BankAccount.IBAN 
                        + ") abgebucht."
                         , fontSmall, XBrushes.Black, totaltext, XStringFormats.TopLeft);

                     //Start saving PDF on server

                     //Create Directory For Customer if needed
                     if (Directory.Exists(targetdir) == false)
                         Directory.CreateDirectory(targetdir);
                   

                     //Check if PDF File already Exists 
                     //TODO: Throw Exception
                     if (File.Exists(b.PDFPath))
                     {
                         Console.WriteLine("Error, Bill already Exists at: " + b.PDFPath);
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
