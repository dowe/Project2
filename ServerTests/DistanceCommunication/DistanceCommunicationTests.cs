using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Server.DistanceCommunication.Tests
{
    [TestClass()]
    public class DistanceCommunicationTests
    {
        [TestMethod()]
        public void CalculateDistanceInKmTest()
        {
            var container = DistanceCommunication.CalculateDistanceInKm(new GPSPosition(48.38403f, 8.14326f),
                new GPSPosition(48.457616f, 7.942638f));
            Assert.IsTrue(container.Distance > 29.0f && container.Distance < 31.0f);
            Assert.IsTrue(container.Time > 0.5f && container.Time < 0.6f);
        }

        [TestMethod()]
        public void CalculateDistanceInKmTest1()
        {
            var container = DistanceCommunication.CalculateDistanceInKm(new GPSPosition(48.38403f, 8.14326f),
                new Address()
                {
                    City = "Offenburg",
                    PostalCode = "77652",
                    Street = "Badstraße 24"
                });
            Assert.IsTrue(container.Distance > 29.0f && container.Distance < 31.0f);
            Assert.IsTrue(container.Time > 0.5f && container.Time < 0.6f);
        }
    }
}
