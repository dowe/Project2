using ASPServer.Models;
using Common.Communication.Client;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ASPServer.Controllers
{
    public class HomeController : Controller
    {
        IClientConnection _clientConnection;

        /// <summary>
        ///     Custom Constructor
        ///     Injected with SimpleInjector
        /// </summary>
        /// <param name="clientConnection">The connection Instance (SignalR)</param>
        public HomeController(IClientConnection clientConnection)
        {
            this._clientConnection = clientConnection;
        }

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

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {
            // Test "DB"
            List<UserModel> registeredUsers = new List<UserModel>();
            registeredUsers.Add(new UserModel { ID = "ich", Password = "asdf" });
            registeredUsers.Add(new UserModel { ID = "du", Password = "1234" });

            // Check if the user exists and if the password is correct
            if (registeredUsers.FirstOrDefault(user => (user.ID == userModel.ID) && user.Password == userModel.Password) != null)
            {
                // Make a crypto key to authenticate the user (much better than a GUID ;-))
                RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();
                byte[] myKey = new byte[48];
                rngProvider.GetBytes(myKey);
                string authId = null;
                myKey.ToList().ForEach(b => authId += b.ToString("x2"));

                // Save the auth key in the session and in the cookie
                Session["authID"] = authId;
                var cookie = new HttpCookie("authId");
                cookie.Value = authId;
                Response.Cookies.Add(cookie);

                // Also save the "real" userID -> important to know for showing user specific data
                Session["userID"] = userModel.ID;
                //UserDB.loginMapping.Add(authId, userModel);

                return RedirectToAction("LoginSuccessful");
            }
            else
            {
                return RedirectToAction("LoginFailed");
            }

        }

        // GET: LoginFailed
        public ActionResult LoginFailed()
        {
            return View();
        }

        // GET: LoginSuccessful
        public ActionResult LoginSuccessful()
        {
            if (this.IsUserAuthenticated())
            {
                return View(new UserModel { ID = Session["userID"].ToString() });
            }
            else
            {
                return RedirectToAction("LoginFailed");
            }
        }

        // GET: Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        /// <summary>
        ///     Checks if the user is logged in
        /// </summary>
        /// <returns>True if the user is authenticated, otherwise false</returns>
        private bool IsUserAuthenticated()
        {
            try
            {
                if (Request.Cookies["authId"].Value == Session["authId"].ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}
