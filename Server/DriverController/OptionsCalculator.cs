using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class OptionsCalculator
    {

        private IRouteDistanceCalculator distanceCalculator = null;

        public OptionsCalculator(IRouteDistanceCalculator distanceCalculator)
        {
            this.distanceCalculator = distanceCalculator;
        }

        public IList<DriverSendOption> CalculateOptions(IEnumerable<Driver> allDrivers, IEnumerable<Order> allUnfinishedOrders,
            IDistanceMatrixPlace destination)
        {
            var driverToAssignedUnfinishedOrders = new Dictionary<Driver, IEnumerable<Order>>();
            foreach (Driver driver in allDrivers)
            {
                driverToAssignedUnfinishedOrders.Add(driver, allUnfinishedOrders.Where(o => o.Driver.UserName.Equals(driver.UserName)));
            }

            var calcDistanceTasks = new List<Task<DriverSendOption>>();
            foreach (KeyValuePair<Driver, IEnumerable<Order>> orders in driverToAssignedUnfinishedOrders)
            {
                calcDistanceTasks.Add(CalculateDistance(orders.Key, orders.Value, destination));
            }

            var options = new List<DriverSendOption>();
            foreach (Task<DriverSendOption> calcTask in calcDistanceTasks)
            {
                calcTask.Wait();
                options.Add(calcTask.Result);
            }

            return options;
        }

        private async Task<DriverSendOption> CalculateDistance(Driver driver, IEnumerable<Order> hisOrders, IDistanceMatrixPlace destination)
        {
            var waypoints = new List<IDistanceMatrixPlace>();
            foreach (Order o in hisOrders)
            {
                // TODO Add support for emergency positions.
                waypoints.Add(new DistanceMatrixAddress(o.Customer.Address));
            }
            waypoints.Add(destination);

            DistanceContainer totalLeftDistance =
                await distanceCalculator.CalculateRouteDistance(waypoints);
            DriverSendOption option = new DriverSendOption(driver, totalLeftDistance);

            return option;
        }
    }
}
