using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPServer.Models;
using Common.DataTransferObjects;

namespace ASPServer.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Bill()
        {
            //TODO: CHECK FOR LOGIN
            var billsRaw = new List<Bill>();
            if (Session["bills"] == null)
            {
                //TODO:EXCHANGE WITH COMM
                billsRaw = new List<Bill>()
            {
                new Bill(){Customer = null, Date = new DateTime(123,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},
                new Bill(){Customer = null, Date = new DateTime(133,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},
                new Bill(){Customer = null, Date = new DateTime(144,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},
                new Bill(){Customer = null, Date = new DateTime(155,1,1), PDFPath = "/App_Data/Erste_Schritte.pdf"},

            };

                if (billsRaw == null || !billsRaw.Any())
                    return View();

                Session["bills"] = billsRaw;
            }


            var bills = new List<BillModel>();
            foreach (var rawBill in billsRaw)
            {
                bills.Add(new BillModel()
                {
                    Date = rawBill.Date,
                    PDFPath = rawBill.PDFPath
                });
            }

            return View(bills);
        }



        [HttpPost]
        public ActionResult Bill(string submitBtn)
        {
            //TODO: CHECK FOR LOGIN

            var bills = (List<Bill>)Session["bills"];
            if (bills == null)
                return View();

            foreach (var bill in bills)
            {
                var date = bill.Date.ToShortDateString();
                if (Request.Form[date] != null)
                {
                    return File(bill.PDFPath, "application/pdf");
                }
            }
            return View();
        }
    }
}