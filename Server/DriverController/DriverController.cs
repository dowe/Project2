using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class DriverController : IDriverController
    {

        private IRouteDistanceCalculator calculator = null;

        public DriverController(IRouteDistanceCalculator routeCalculator)
        {
            calculator = routeCalculator;
        }

        public Driver DetermineDriverOrNull(IList<Driver> allDrivers, Address destination)
        {
            throw new NotImplementedException();
        }

        public Driver DetermineDriverOrNull(IList<Driver> allDrivers, GPSPosition destination)
        {
            throw new NotImplementedException();
        }
    }
}
