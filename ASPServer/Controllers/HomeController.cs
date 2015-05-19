using ASPServer.Models;
using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace ASPServer.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionBills = "billsData";
        private const string AuthID = "authId";
        private const string UserID = "userID";
        private const string SessionAnalysisData = "analysisData";
        private const string SessionAnalysesList = "analysesList";
        private const string SessionOrdered = "orderedAnalysisList";
        private const string SessionPatientsList = "patientsList";
        private const string LastActionTime = "lastActionTime";
        private const int LogoutIdleTimeInSec = 300;

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

        public ActionResult Order()
        {
            //User logged in?
            if (!this.IsUserAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var orderModel = new OrderModel();

            //Get Analysis-List
            if (Session[SessionAnalysisData] == null)
            {
                Session[SessionAnalysisData] = _clientConnection.SendWait<CmdReturnGetAnalyses>(new CmdGetAnalyses()).Analyses.ToList();
                //populate analysis listbox
                foreach (var analysis in (List<Analysis>)Session[SessionAnalysisData])
                {
                    orderModel.AnalysisList.Add(new SelectListItem() { Text = analysis.Name, Value = analysis.Name });
                }
                Session[SessionAnalysesList] = orderModel.AnalysisList;
            }

            //incase site was used before
            orderModel.PatientsList = (List<SelectListItem>)Session[SessionPatientsList] ?? new List<SelectListItem>();
            orderModel.AnalysisList = (List<SelectListItem>)Session[SessionAnalysesList] ?? new List<SelectListItem>();
            orderModel.OrderedItems = (Dictionary<string, List<Analysis>>)Session[SessionOrdered] ?? new Dictionary<string, List<Analysis>>();

            return View(orderModel);
        }

        [HttpPost]
        public ActionResult Order(OrderModel orderModel)
        {
            if (!this.IsUserAuthenticated())
            {
                return RedirectToAction("Login");
            }

            orderModel.PatientsList = (List<SelectListItem>)Session[SessionPatientsList];
            orderModel.AnalysisList = (List<SelectListItem>)Session[SessionAnalysesList];
            orderModel.OrderedItems = (Dictionary<string, List<Analysis>>)Session[SessionOrdered];

            //add patient button
            if (Request.Form["NewPatientButton"] != null)
            {
                var pats = new List<SelectListItem>();
                if (Session[SessionPatientsList] != null)
                {
                    pats = Session[SessionPatientsList] as List<SelectListItem>;
                }
                if (!orderModel.NewPatient.IsNullOrWhiteSpace() && pats.FirstOrDefault(s => s.Text == orderModel.NewPatient) == null)
                {
                    pats.Add(new SelectListItem() { Text = orderModel.NewPatient, Value = orderModel.NewPatient });
                }


                //prepare model for return
                Session[SessionPatientsList] = pats;
                orderModel.PatientsList = (List<SelectListItem>)Session[SessionPatientsList];
            }
            else if (Request.Form["OrderButton"] != null)
            {
                //Order everything
                var orderItems = (Dictionary<string, List<Analysis>>)Session[SessionOrdered];
                _clientConnection.Send(new CmdAddOrder(orderItems, (string)Session[UserID]));

                //remove ordered items from model and session
                Session[SessionOrdered] = null;
                orderModel.OrderedItems = null;
            }
            else if (Request.Form["AddAnalysisButton"] != null)
            {
                //add Test button
                var orderItems = new Dictionary<string, List<Analysis>>();
                if (Session[SessionOrdered] != null)
                    orderItems = (Dictionary<string, List<Analysis>>)Session[SessionOrdered];

                if (orderModel.SelectedPatient != null && orderModel.SelectedAnalysis != null)
                {
                    foreach (var pat in orderModel.SelectedPatient)
                    {
                        if (!orderItems.ContainsKey(pat))
                        {
                            orderItems.Add(pat, new List<Analysis>());
                        }

                        foreach (var analysisName in orderModel.SelectedAnalysis)
                        {
                            var analysesData = Session[SessionAnalysisData] as List<Analysis>;
                            orderItems[pat].Add(analysesData.Where(a => a.Name == analysisName).FirstOrDefault());
                        }
                    }
                }

                //preapre model for return
                Session[SessionOrdered] = orderItems;
                orderModel.OrderedItems = (Dictionary<string, List<Analysis>>)Session[SessionOrdered];

            }
            else
            {
                bool empty = false;
                //Delete
                foreach (var orderItem in (Dictionary<string, List<Analysis>>)Session[SessionOrdered])
                {
                    foreach (var analysis in (List<Analysis>)orderItem.Value)
                    {
                        string str = "Delete#" + orderItem.Key + "#" + analysis.Name;
                        if (Request.Form[str] != null)
                        {
                            orderItem.Value.Remove(analysis);
                            if (orderItem.Value.Count == 0)
                            {
                                ((Dictionary<string, List<Analysis>>)Session[SessionOrdered]).Remove(orderItem.Key);
                                empty = true;
                            }
                            break;
                        }
                    }
                    if (empty)
                        break;
                }
                orderModel.OrderedItems = (Dictionary<string, List<Analysis>>)Session[SessionOrdered];
            }

            return View(orderModel);
        }

        public ActionResult Bill()
        {
            if (!this.IsUserAuthenticated())
            {
                return RedirectToAction("Login");
            }

            var billsRaw = new List<Bill>();
            if (Session[SessionBills] == null)
            {
                var cmd =
                    _clientConnection.SendWait<CmdReturnGetAllBillsOfUser>(
                        new CmdGetAllBillsOfUser((string)Session[UserID]));

                if (!cmd.Bills.Any())
                    return View();

                billsRaw = cmd.Bills.ToList();
                Session[SessionBills] = billsRaw;
            }
            else
            {
                billsRaw = (List<Bill>)Session[SessionBills];
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
            if (!this.IsUserAuthenticated())
            {
                return RedirectToAction("LoginFailed");
            }


            var bills = (List<Bill>)Session[SessionBills];
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

        // GET: Result
        public ActionResult Result()
        {
            if (!this.IsUserAuthenticated())
            {
                return RedirectToAction("Login");
            }

            // get all User orders
            CmdReturnGetUsersOrderResults cmd = this._clientConnection.SendWait<CmdReturnGetUsersOrderResults>(new CmdGetUsersOrderResults(Session[UserID].ToString()));
            IReadOnlyCollection<Order> rawResults = cmd.Orders;

            // Copy the data into the ResultModel
            List<ResultModel> results = new List<ResultModel>();
            ResultModel resultModel;
            foreach (Order order in rawResults)
            {
                foreach (Test test in order.Test)
                {
                    resultModel = new ResultModel();
                    resultModel.OrderNo = order.OrderID;
                    resultModel.Patient = test.PatientID;
                    resultModel.Analysis = test.Analysis.Name;
                    resultModel.Status = test.TestState;
                    resultModel.ResultValue = test.ResultValue;
                    resultModel.MinCritValue = test.Analysis.ExtremeMinValue;
                    resultModel.MaxCritValue = test.Analysis.ExtremeMaxValue;
                    resultModel.UnitOfMeasure = test.Analysis.UnitOfMeasure;
                    resultModel.Critical = test.Critical;
                    results.Add(resultModel);
                }
            }

            // Add some example Data
            results.Add(new ResultModel() { Patient = "3756", Analysis = "Blut", UnitOfMeasure = "mg" });
            results.Add(new ResultModel() { Analysis = "Speichel", UnitOfMeasure = "mg" });
            results.Add(new ResultModel() { Analysis = "Urin", Critical = true, UnitOfMeasure = "ml" });

            return View(results);
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
            CmdReturnLoginCustomer cmd = this._clientConnection.SendWait<CmdReturnLoginCustomer>(new CmdLoginCustomer(userModel.ID, userModel.Password));

            // Check if the user exists and if the password is correct
            if (cmd.Success)
            {
                // Make a crypto key to authenticate the user (much better than a GUID ;-))
                RNGCryptoServiceProvider rngProvider = new RNGCryptoServiceProvider();
                byte[] myKey = new byte[48];
                rngProvider.GetBytes(myKey);
                string authId = null;
                myKey.ToList().ForEach(b => authId += b.ToString("x2"));

                // Save the auth key in the session and in the cookie
                Session[AuthID] = authId;
                var cookie = new HttpCookie(AuthID);
                cookie.Value = authId;
                Response.Cookies.Add(cookie);

                // Save the login timestamp
                long now = DateTime.UtcNow.Ticks;
                Session[LastActionTime] = now;

                // Also save the "real" userID -> important to know for showing user specific data
                Session[UserID] = userModel.ID;

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
                return View(new UserModel { ID = Session[UserID].ToString() });
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
                if (Request.Cookies[AuthID].Value == Session[AuthID].ToString())
                {
                    // Check how long since the last user action in a authenticated context
                    TimeSpan span = new TimeSpan(DateTime.UtcNow.Ticks - ((long)Session[LastActionTime]));
                    if (span.Seconds < LogoutIdleTimeInSec)
                    {
                        // If the user was within the time we set the new time
                        Session[LastActionTime] = DateTime.UtcNow.Ticks;
                        return true;
                    }
                    else
                    {
                        // if the user timed out we clear the whole session
                        Session.Clear(); ;
                    }
                }

                return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }
}
