using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Common.DataTransferObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.DistanceCalculation;
using Server.DriverControlling;

namespace ServerTests.DriverControlling
{
    [TestClass]
    public class DriverControllerIntegrationTests
    {

        private readonly Address HOME_ADDRESS = new Address("Badstrasse 24", "77652", "Offenburg");

        private DriverController CreateTestee(Address home, TimeSpan collectOrderTimeLimit)
        {
            DriverControllerSettings settings = new DriverControllerSettings(new DistanceMatrixAddress(home),
                collectOrderTimeLimit);
            return new DriverController(settings);
        }

        private Driver CreateDriver(string username)
        {
            return new Driver
            {
                UserName = username
            };
        }

        private Order CreateOrder(Address address, Driver driver)
        {
            return new Order
            {
                Customer = new Customer
                {
                    Address = address,
                },
                Driver = driver
            };
        }

        [TestMethod]
        public void DetermineDriverOrNull_WithMultiplePossibleSolutions_ReturnsFastest()
        {
            List<Car> cars = new List<Car>();
            List<Order> orders = new List<Order>();
            // Slow because has order
            Driver driverSlowHasOrder = CreateDriver("slowHasOrder");
            Car carSlowHasOrder = new Car()
            {
                CurrentDriver = driverSlowHasOrder,
                LastPosition = new GPSPosition() { Latitude = 48.4580221f, Longitude = 7.9423354f }, // Home
                Roadworthy = true
            };
            cars.Add(carSlowHasOrder);
            Order orderSlowHasOrder = CreateOrder(new Address("Haydnstraße 3", "77694", "Kehl"), driverSlowHasOrder);
            orders.Add(orderSlowHasOrder);
            // Slow but possible (to far away)
            Driver driverSlowToFarAway = CreateDriver("slowToFarAway");
            Car carSlowToFarAway = new Car
            {
                CurrentDriver = driverSlowToFarAway,
                LastPosition = new GPSPosition() {Latitude = 48.994099f, Longitude = 8.398917f}, // Karlsruhe Hbf
                Roadworthy = true
            };
            cars.Add(carSlowToFarAway);
            // Fast and possible
            Driver driverFastAndPossible = CreateDriver("fastAndPossible");
            Car carFastAndPossible = new Car
            {
                CurrentDriver = driverFastAndPossible,
                LastPosition = new GPSPosition() {Latitude = 48.4580221f, Longitude = 7.9423354f}, // Home
                Roadworthy = true
            };
            cars.Add(carFastAndPossible);

            Address destination = new Address("Okenstrasse 105", "77652", "Offenburg");
            DriverController testee = CreateTestee(HOME_ADDRESS, TimeSpan.FromHours(5));
            Driver optimalDriver = testee.DetermineDriverOrNull(cars, orders, new DistanceMatrixAddress(destination));

            Assert.AreEqual(driverFastAndPossible, optimalDriver);
        }
    }
}
