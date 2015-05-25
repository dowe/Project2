using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DatabaseCommunication;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.DataTransferObjects;


namespace Server.DatabaseCommunication.Tests
{
    [TestClass()]
    public class DatabaseCommunicatorTests
    {
        public DatabaseCommunicator Com;

        [TestInitialize]
        public void Initialize()
        {
            Com = new DatabaseCommunicator();
        }

        [TestCleanup]
        public void Cleanup() //löscht die Datenbank
        {
            LaborContext con = new LaborContext();
            con.Database.Delete();
        }

        [TestMethod()]
        public void GetAllAnalysisTest()
        {
            //Com.GetAllAnalysis();
        }

        [TestMethod()]
        public void ShiftSchedulesTest()
        {
            Com.StartTransaction();
            ShiftSchedule schedule = new ShiftSchedule() { Date = new DateTime(2015,5,1), DayEntry = new List<DayEntry>(){ new DayEntry(){ Date = DateTime.Now} } };
            Com.CreateShiftSchedule(schedule);
            Com.EndTransaction(TransactionEndOperation.SAVE);

            Com.StartTransaction();
            List<ShiftSchedule> Schedules = Com.GetShiftSchedules();
            

            Assert.AreEqual(Schedules.Count, 1);
            Assert.AreEqual(Schedules.First().Date, new DateTime(2015, 5, 1));
            Assert.AreEqual(Schedules.First().DayEntry.Count, 1);
            Com.EndTransaction(TransactionEndOperation.READONLY);

            Com.StartTransaction();
            ShiftSchedule schedule2 = new ShiftSchedule() { Date = new DateTime(2015, 6, 1) };
            Com.CreateShiftSchedule(schedule2);
            Com.EndTransaction(TransactionEndOperation.SAVE);

            Com.StartTransaction();
            Schedules = Com.GetShiftSchedules();
            Com.EndTransaction(TransactionEndOperation.READONLY);

            Assert.AreEqual(Schedules.Count, 2);
        }

        //[TestMethod()]
        //public void GetAllOrdersTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetOrderTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetTestTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void EndTransactionTest()
        {
            Com.EndTransaction(TransactionEndOperation.SAVE);
            Assert.Fail();
        }

        [TestMethod()]
        public void StartTransactionTest()
        {
            Com.StartTransaction();
            Com.EndTransaction(TransactionEndOperation.READONLY);
        }

        [TestMethod()]
        public void CreateCustomerTest()
        {
            Com.StartTransaction();
            Customer customer = new Customer();
            customer.FirstName = "Peter";
            customer.Label = "Enis";
            customer.UserName = "Penis";
            Com.CreateCustomer(customer);
            Com.EndTransaction(TransactionEndOperation.SAVE);
        }

        //[TestMethod()]
        //public void GetAllTestsTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetAllCarsTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetAllCustomerTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetCustomerTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetCustomerAddressTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CreateOrderTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetAllBillsTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetBillDatesOfMonthTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetBillTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetAllDriverTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetDriverTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetCarbyIdTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetLastCarLogbookEntryTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void CreateCarLogbookEnryTest()
        //{
        //    Assert.Fail();
        //}
    }
}
