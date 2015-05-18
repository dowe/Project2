using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class DriverController : IDriverController
    {

        private OptionsCalculator routeCalculator = null;
        private OptionsEvaluator evaluator = null;

        private IDistanceMatrixPlace home = null;

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
            var calcDistanceTasks = new List<Task<DriverSendOption>>();
            foreach (Driver driver in allDrivers)
            {
                var driversOrders = allUnfinishedOrders.Where(o => o.Driver.UserName.Equals(driver.UserName));
                calcDistanceTasks.Add(routeCalculator.CalculateDistance(driver, driversOrders, destination));
            }

            // Route calculation is done parallel.
            var options = new List<DriverSendOption>();
            foreach (Task<DriverSendOption> calcTask in calcDistanceTasks)
            {
                calcTask.Wait();
                options.Add(calcTask.Result);
            }

            var bestOptionOrNull = evaluator.ChooseBestOptionOrNull(options);
            Driver optimalDriver = null;
            if (bestOptionOrNull != null)
            {
                optimalDriver = bestOptionOrNull.Driver;
            }

            return optimalDriver;
        }

    }
}
