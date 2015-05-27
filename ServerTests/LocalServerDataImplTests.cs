using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace Server.Tests
{
    [TestClass]
    public class LocalServerDataImplTests
    {
        [TestMethod]
        public void LocalServerDataImplTest()
        {
            LocalServerDataImpl testee = new LocalServerDataImpl();

            Assert.IsNotNull(testee.TimerList);
            Assert.IsNotNull(testee.ZmsAddress);

            foreach (char c in testee.TaxiPhoneNumber)
            {
                Assert.IsTrue(Char.IsNumber(c));
            }
        }
    }
}
