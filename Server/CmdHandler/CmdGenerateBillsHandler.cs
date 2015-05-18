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

namespace Server.CmdHandler
{
    public class CmdGenerateBillsHandler : CommandHandler<CmdGenerateBills>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;
        List<Order> _OrderList;
        IList<Bill> _BillList;
        IList<Test> _TestList;
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
             DateTime now = DateTime.Now;
             Bill b;
             int OrderAmount;
             for (int i = 0; i < _OrderList.Count; i++)
             {
                 //New Customer whose Oders havent been invoiced
                 if (!_OrderList[i].Invoiced)
                 {
                     _TestList = new List<Test>();
                     OrderAmount = 1;
                     //Create new Bill
                     b = new Bill();
                     b.Customer = _OrderList[i].Customer;
                     b.Date = now;
                     b.PDFPath = "/App_Data/" + _OrderList[i].Customer.UserName + "/" + now.ToString("dd-MM-yyyy");
                     //Add Testlist from Order
                     _TestList.Concat(_OrderList[i].Test);

                     //Check if Customer has other Orders pending
                     for (int j = i; j < _OrderList.Count; j++)
                     {
                         if (_OrderList[j].Customer.UserName.Equals(b.Customer.UserName) && _OrderList[j].Invoiced == false)
                         {
                             OrderAmount++;
                             _OrderList[j].Invoiced = true;
                             _TestList.Concat(_OrderList[j].Test);
                         }
                     }
                     //now all Tests and the amount of Orders should be gathered correctly for the Customer


                     //generate PDF
                     PdfDocument doc = new PdfDocument();
                     PdfPage page = doc.AddPage();
                     page.Size = PdfSharp.PageSize.A4;
                     double y = 10;
                     XGraphics gfx = XGraphics.FromPdfPage(page);

                     double marginLeft = 10;

                     //Fonts and Fontheights
                     double fontHeightSmall = 18;
                     double fontHeightBig = 22;
                     XFont fontSmallBold = new XFont("Arial", fontHeightSmall, XFontStyle.Bold);
                     XFont fontBig = new XFont("Arial", fontHeightBig);
                     XFont fontSmall = new XFont("Arial", fontHeightSmall);



                     //Start Headlines
                     XRect headline1 = new XRect(marginLeft, y, page.Width, fontHeightBig);
                     gfx.DrawString("Rechnung vom " + now.ToString("dd-MM-yyyy"), fontBig, XBrushes.Black, headline1);
                     y += fontHeightBig;


                     XRect headline2 = new XRect(marginLeft, y, page.Width, fontHeightSmall);
                     gfx.DrawString("Für den Zeitraum vom " + now.AddDays(-8).ToString("dd-MM-yyyy") + "bis" + now.AddDays(-1).ToString("dd-MM-yyyy"), fontSmall, XBrushes.Black, headline2);
                     y += fontHeightSmall;

                     XRect orders = new XRect(marginLeft, y, page.Width, fontHeightSmall * 2 + 10);
                     gfx.DrawString(OrderAmount.ToString() + " Fahrt(en)", fontSmall, XBrushes.Black, orders);
                     gfx.DrawString(b.Customer.TwoWayRoadCostInEuro.ToString(),fontSmall, XBrushes.Black, orders, XStringFormats.Center);


                     doc.Save(b.PDFPath);
                     Process.Start(b.PDFPath);

                 }

             }








             //Create a List of Bills with one Bill per Customer, even if there a several Orders from one Customer

            /* foreach (Order o in _OrderList)
             {
                 bool _CustomerExists = false;

                 //check if there is already a Bill for this Customer
                 foreach(Bill b in _BillList)
                 {
                     if (o.Customer.Equals(b.Customer))
                     {
                         _CustomerExists = true;
                     }
                 }

                 //If there is no Bill yet create a new one in List
                 if (_CustomerExists == false)
                 {
                     _BillList.Add(new Bill());
                     _BillList[_BillList.Count - 1].Date = now;
                     _BillList[_BillList.Count - 1].Customer = o.Customer;
                     _BillList[_BillList.Count - 1].PDFPath = "/App_Data/" + o.Customer + "/" + now.ToString("dd-MM-yyyy");
                 }
             }

             int i = 0;
             //Use BillList to create PDFs 
             foreach (Order o in _OrderList.FindAll(c => (c.Customer.UserName == _BillList[i].Customer.UserName)))
             {

             }
             */
         }

    }
}
