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

        private Func<GPSPosition, GPSPosition, DistanceContainer> calculateDistanceToPositionInKm = null;
        private Func<GPSPosition, Address, DistanceContainer> calculateDistanceToAddressInKm = null;

        public DriverController()
            : this(DistanceCalculation.DistanceCalculation.CalculateDistanceInKm, DistanceCalculation.DistanceCalculation.CalculateDistanceInKm)
        {
        }

        public DriverController(Func<GPSPosition, GPSPosition, DistanceContainer> calculateDistanceToPositionInKm,
            Func<GPSPosition, Address, DistanceContainer> calculateDistanceToAddressInKm)
        {
            this.calculateDistanceToPositionInKm = calculateDistanceToPositionInKm;
            this.calculateDistanceToAddressInKm = calculateDistanceToAddressInKm;
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
