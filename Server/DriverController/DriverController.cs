using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
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

        public Driver DetermineDriverOrNull(IDatabaseCommunicator db, GPSPosition position)
        {
            IDistanceMatrixPlace destination = new DistanceMatrixGPSPosition(position);

            return DetermineDriverOrNull(db, destination);
        }

        public Driver DetermineDriverOrNull(IDatabaseCommunicator db, Address address)
        {
            IDistanceMatrixPlace destination = new DistanceMatrixAddress(address);

            return DetermineDriverOrNull(db, destination);
        }

        private Driver DetermineDriverOrNull(IDatabaseCommunicator db, IDistanceMatrixPlace destination)
        {
            db.StartTransaction();

            IEnumerable<Car> allOccupiedCars = db.GetAllCars(c => c.CurrentDriver != null);
            IEnumerable<Order> allUnfinishedOrders = db.GetAllOrders(o => o.CollectDate == null);

            db.EndTransaction(TransactionEndOperation.READONLY);

            return DetermineDriverOrNull(allOccupiedCars, allUnfinishedOrders, destination);
        }

        private Driver DetermineDriverOrNull(IEnumerable<Car> allOccupiedCars, IEnumerable<Order> allUnfinishedOrders,
            IDistanceMatrixPlace destination)
        {
            var calcDistanceTasks = new List<Task<DriverSendOption>>();
            foreach (Car car in allOccupiedCars)
            {
                var driversOrders = allUnfinishedOrders.Where(o => o.Driver.UserName.Equals(car.CurrentDriver.UserName));
                calcDistanceTasks.Add(routeCalculator.CalculateDistance(car, driversOrders, destination));
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
                optimalDriver = bestOptionOrNull.Car.CurrentDriver;
            }

            return optimalDriver;
        }

    }
}
