using System;
using System.Collections.Generic;
using Common.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DistanceCalculation;

namespace ServerTests.DistanceCalculation
{
    [TestClass]
    public class DirectionsRouteDistanceCalculatorTests
    {

        private DirectionsRouteDistanceCalculator CreateTestee()
        {
            return new DirectionsRouteDistanceCalculator(false);
        }

        [TestMethod]
        public void CalculateDistance_OnPassMixedPlaces_ReturnsRealisticDistance()
        {
            var testee = CreateTestee();
            var places = new List<IDistanceMatrixPlace>()
            {
                new DistanceMatrixGPSPosition(new GPSPosition
                {
                    Latitude = 48.476483f,
                    Longitude = 7.945695f
                }),
                new DistanceMatrixAddress(new Address
                {
                    City = "Offenburg",
                    PostalCode = "77652",
                    Street = "Badstrasse 24"
                }),
                new DistanceMatrixGPSPosition(new GPSPosition
                {
                    Latitude = 48.476483f,
                    Longitude = 7.945695f
                })
            };

            DistanceContainer distance = testee.CalculateRouteDistance(places);
            Assert.AreEqual(0.23f, distance.Time, 0.1f);
            Assert.AreEqual(5.8f, distance.Distance, 1f);
        }
    }
}
