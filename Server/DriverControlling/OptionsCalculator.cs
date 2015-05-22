using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverControlling
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
            waypoints.Add(new DistanceMatrixGPSPosition(car.LastPosition));
            foreach (Order o in hisOrders)
            {
                if (o.EmergencyPosition != null)
                {
                    waypoints.Add(new DistanceMatrixGPSPosition(o.EmergencyPosition));
                }
                else
                {
                    waypoints.Add(new DistanceMatrixAddress(o.Customer.Address));
                }
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
