using System.Collections.Generic;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class DriverController : IDriverController
    {

        private OptionsCalculator routeCalculator = null;
        private OptionsEvaluator evaluator = null;

        public DriverController(OptionsCalculator routeCalculator, OptionsEvaluator evaluator)
        {
            this.routeCalculator = routeCalculator;
            this.evaluator = evaluator;
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
            IList<DriverSendOption> options = routeCalculator.CalculateOptions(allDrivers, allUnfinishedOrders, destination);

            return null;
        }

    }
}
