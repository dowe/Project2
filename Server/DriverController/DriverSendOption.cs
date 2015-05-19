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

        public Car Car { get; private set; }
        public DistanceContainer TotalLeftDistance { get; private set; }

        public DriverSendOption(Car car, DistanceContainer totalLeftDistance)
        {
            Car = car;
            TotalLeftDistance = totalLeftDistance;
        }

    }
}
