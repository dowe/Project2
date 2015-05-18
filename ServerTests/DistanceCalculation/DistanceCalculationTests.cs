using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Server.DistanceCalculation.Tests
{

    [TestClass()]
    public class DistanceCalculationTests
    {
        

        [TestMethod()]
        public void CalculateDistanceInKmTest()
        {
            var container = DistanceCalculation.CalculateDistanceInKm(new GPSPosition{Latitude = 48.48548f, Longitude = 7.94121f},
                new GPSPosition{Latitude = 48.4580221f, Longitude = 7.9423354f});
            Assert.IsTrue(container.Distance > 3.5f && container.Distance < 4.0f);
            Assert.IsTrue(container.Time > 0.13f && container.Time < 0.14f);
        }

        [TestMethod()]
        public void CalculateDistanceInKmTest1()
        {
            var container = DistanceCalculation.CalculateDistanceInKm(new GPSPosition{Latitude = 48.48548f, Longitude = 7.94121f},
                new Address()
                {
                    City = "Offenburg",
                    PostalCode = "77652",
                    Street = "Badstraße 24"
                });
            Assert.IsTrue(container.Distance > 3.5f && container.Distance < 4.0f);
            Assert.IsTrue(container.Time > 0.13f && container.Time < 0.14f);
        }

        [TestMethod()]
        public void CalculateDistanceInKmTest2()
        {
            var container = DistanceCalculation.CalculateDistanceInKm(
                new Address()
                {
                    City = "Offenburg",
                    PostalCode = "77652",
                    Street = "Englerstraße 8"
                },
                new Address()
                {
                    City = "Offenburg",
                    PostalCode = "77652",
                    Street = "Badstraße 24"
                });
            Assert.IsTrue(container.Distance > 3.5f && container.Distance < 4.0f);
            Assert.IsTrue(container.Time > 0.13f && container.Time < 0.14f);
        }
    }
}
