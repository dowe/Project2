using Common.Commands;
using Common.Communication.Client;
using Common.DataTransferObjects;
using ManagementSoftware.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Data;

namespace ManagementSoftware.Tests.ViewModelTests
{
    [TestClass]
    public class DailyStatisticVMTests
    {
        private IClientConnection mockConnection;
        private DailyStatisticVM testee;
        private DailyStatistic dailyStat;

        [TestInitialize]
        public void SetUp()
        {
            mockConnection = Substitute.For<IClientConnection>();
            dailyStat = TestData.CreateDailyStatistic("2/2/2000");

            CmdReturnGetDailyStatistic cmdDailyStat = new CmdReturnGetDailyStatistic(Guid.NewGuid(), dailyStat);
            mockConnection.SendWait<CmdReturnGetDailyStatistic>(Arg.Any<CmdGetDailyStatistic>()).Returns<CmdReturnGetDailyStatistic>((x) => cmdDailyStat);
            testee = new DailyStatisticVM(mockConnection);

        }

        [TestMethod]
        public void testCurrentOrdersData()
        {
            Assert.AreEqual(22, testee.NewOrders);
            Assert.AreEqual(100, testee.CompletedOrders);
            Assert.AreEqual(42, testee.OrdersInProgress);
            Assert.AreEqual(11, testee.TestsCompleted);
            Assert.AreEqual(33, testee.TestsInProgress);
        }
        [TestMethod]
        public void testTimeSpan()
        {
            Assert.AreEqual("01-02-2000 bis 02-02-2000",testee.TimeSpan);
        }
    }
}
