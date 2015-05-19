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

        public async Task<DriverSendOption> CalculateDistance(Car car, IEnumerable<Order> hisOrders, IDistanceMatrixPlace destination, IDistanceMatrixPlace home)
        {
            var waypoints = new List<IDistanceMatrixPlace>();
            foreach (Order o in hisOrders)
            {
                // TODO Add support for emergency positions.
                waypoints.Add(new DistanceMatrixAddress(o.Customer.Address));
            }
            waypoints.Add(destination);
            waypoints.Add(home);

            DistanceContainer totalLeftDistance =
                await distanceCalculator.CalculateRouteDistance(waypoints);
            DriverSendOption option = new DriverSendOption(car, totalLeftDistance);

            return option;
        }
    }
}
