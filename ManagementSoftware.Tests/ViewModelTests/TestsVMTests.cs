using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;
using NSubstitute;
using System.Collections.Generic;
using ManagementSoftware.ViewModel;
using Tests.Data;
namespace ManagementSoftware.Tests.ViewModelTests
{
    [TestClass]
    public class TestsVMTests
    {
        private IClientConnection mockConnection;
        private TestsVM testee;
         
        [TestInitialize]
        public void SetUp()
        {
            mockConnection = Substitute.For<IClientConnection>();
            List<Order> orders = TestData.CreateOrderList();
            CmdReturnGetAllOrders cmdOrders = new CmdReturnGetAllOrders(Guid.NewGuid(), orders);
            mockConnection.SendWait<CmdReturnGetAllOrders>(Arg.Any<CmdGetAllOrders>()).Returns<CmdReturnGetAllOrders>((x) => cmdOrders);

            testee = new TestsVM(mockConnection);
           

        }
        [TestMethod]
        public void TestFirstOrder()
        {
            testee.SelectedTestEntry = testee.DataList[0];
            testee.RefreshDetail();
            Assert.AreEqual("Kein Alarm gesendet", testee.SelectedTestEntry.AlarmState);
            Assert.AreNotEqual(null, testee.SelectedTestEntry);
            Assert.AreEqual("", testee.SelectedTestEntry.BringDate);
        }
    }
}
