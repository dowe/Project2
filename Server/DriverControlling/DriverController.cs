using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using Server.DistanceCalculation;

namespace Server.DriverControlling
{
    public class DriverController : IDriverController
    {

        private OptionsCalculator routeCalculator = null;
        private OptionsEvaluator evaluator = null;

        private DriverControllerSettings settings = null;

        /// <summary>
        /// This ctor is used for testing purpose (dependency injection).
        /// </summary>
        public DriverController(OptionsCalculator routeCalculator, OptionsEvaluator evaluator, DriverControllerSettings settings)
        {
            this.routeCalculator = routeCalculator;
            this.evaluator = evaluator;
            this.settings = settings;
        }

        public DriverController(DriverControllerSettings settings)
            : this(new OptionsCalculator(new DirectionsRouteDistanceCalculator(false)), new OptionsEvaluator(), settings)
        {
            evaluator.AddHardConstraint(MustBeDoneInsideOrderCollectionTimeLimit); // All orders must be finished before 6h passed.
            evaluator.AddHardConstraint(MustBeDoneBeforeChangeOfShift);

            evaluator.SetSoftConstraint(o => -o.TotalLeftDistance.Time); // The faster, the better.
        }

        private bool MustBeDoneInsideOrderCollectionTimeLimit(DriverSendOption option)
        {
            return option.TotalLeftDistance.Time < settings.OrderCollectionTimeLimit.TotalHours;
        }

        private bool MustBeDoneBeforeChangeOfShift(DriverSendOption option)
        {
            DateTime nextChangeOfShift = GetNextChangeOfShift();
            DateTime estimatedFinishOfShift = DateTime.Now.Add(TimeSpan.FromHours(option.TotalLeftDistance.Time));

            return estimatedFinishOfShift.CompareTo(nextChangeOfShift) < 0;
        }

        private DateTime GetNextChangeOfShift()
        {
            DateTime nextChangeOfShift = DateTime.Today;

            if (DateTime.Now.TimeOfDay.TotalHours < 12)
            {
                nextChangeOfShift = nextChangeOfShift.Add(TimeSpan.FromHours(12));
            }
            else
            {
                nextChangeOfShift = nextChangeOfShift.Add(TimeSpan.FromDays(1));
            }

            return nextChangeOfShift;
        }

        public Driver DetermineDriverOrNullInsideTransaction(IDatabaseCommunicator db, GPSPosition position)
        {
            IDistanceMatrixPlace destination = new DistanceMatrixGPSPosition(position);

            return DetermineDriverOrNullInsideTransaction(db, destination);
        }

        public Driver DetermineDriverOrNullInsideTransaction(IDatabaseCommunicator db, Address address)
        {
            IDistanceMatrixPlace destination = new DistanceMatrixAddress(address);

            return DetermineDriverOrNullInsideTransaction(db, destination);
        }

        private Driver DetermineDriverOrNullInsideTransaction(IDatabaseCommunicator db, IDistanceMatrixPlace destination)
        {
            IEnumerable<Car> allOccupiedCars = db.GetAllCars(c => c.CurrentDriver != null);
            IEnumerable<Order> allUnfinishedOrders = db.GetAllOrders(o => o.CollectDate == null && o.Driver != null);

            return DetermineDriverOrNull(allOccupiedCars, allUnfinishedOrders, destination);
        }

        public Driver DetermineDriverOrNull(IEnumerable<Car> allOccupiedCars, IEnumerable<Order> allUnfinishedOrders,
            IDistanceMatrixPlace destination)
        {
            var calcDistanceTasks = new List<Task<DriverSendOption>>();
            foreach (Car car in allOccupiedCars)
            {
                var driversOrders = allUnfinishedOrders.Where(o => o.Driver.UserName.Equals(car.CurrentDriver.UserName));
                calcDistanceTasks.Add(routeCalculator.CalculateDistance(car, driversOrders, destination, settings.Home));
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
