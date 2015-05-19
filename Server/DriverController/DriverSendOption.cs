using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverController
{
    public class DriverSendOption
    {

        public Driver Driver { get; private set; }
        public DistanceContainer TotalLeftDistance { get; private set; }

        public DriverSendOption(Driver driver, DistanceContainer totalLeftDistance)
        {
            Driver = driver;
            TotalLeftDistance = totalLeftDistance;
        }

    }
}
