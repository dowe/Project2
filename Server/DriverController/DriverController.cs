using System.Collections.Generic;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class DriverController : IDriverController
    {

        private OptionsCalculator routeCalculator = null;
        private OptionsEvaluator evaluator = null;

        /// <summary>
        /// This ctor is used for testing purpose (dependency injection).
        /// </summary>
        /// <param name="routeCalculator"></param>
        /// <param name="evaluator"></param>
        public DriverController(OptionsCalculator routeCalculator, OptionsEvaluator evaluator)
        {
            this.routeCalculator = routeCalculator;
            this.evaluator = evaluator;
        }

        public DriverController()
            : this(new OptionsCalculator(new DirectionsRouteDistanceCalculator(false)), new OptionsEvaluator())
        {
            evaluator.AddHardConstraint(o => o.TotalLeftDistance.Time < 6); // All orders must be finished before 6h passed.
            // TODO Add shift time hard constraint.
            evaluator.SetSoftConstraint(o => -o.TotalLeftDistance.Time); // The faster, the better.
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
