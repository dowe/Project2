using System.Collections.Generic;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class DriverController : IDriverController
    {

        private OptionsCalculator calculator = null;

        public DriverController(OptionsCalculator routeCalculator)
        {
            calculator = routeCalculator;
        }

        public Driver DetermineDriverOrNull(IEnumerable<Driver> allDrivers, IEnumerable<Order> allUnfinishedOrders, Address destination)
        {
            return DetermineDriverOrNull(allDrivers, allUnfinishedOrders, new DistanceMatrixAddress(destination));
        }

        public Driver DetermineDriverOrNull(IEnumerable<Driver> allDrivers, IEnumerable<Order> allUnfinishedOrders, GPSPosition destination)
        {
            return DetermineDriverOrNull(allDrivers, allUnfinishedOrders, new DistanceMatrixGPSPosition(destination));
        }

        public Driver DetermineDriverOrNull(IEnumerable<Driver> allDrivers, IEnumerable<Order> allUnfinishedOrders,
            IDistanceMatrixPlace destination)
        {
            IList<DriverSendOption> options = calculator.CalculateOptions(allDrivers, allUnfinishedOrders, destination);

            return null;
        }

    }
}
