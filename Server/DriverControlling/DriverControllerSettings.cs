using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DistanceCalculation;

namespace Server.DriverControlling
{
    public class DriverControllerSettings
    {

        public IDistanceMatrixPlace Home { get; private set; }
        public TimeSpan OrderCollectionTimeLimit { get; private set; }

        public DriverControllerSettings(IDistanceMatrixPlace home, TimeSpan orderCollectionTimeLimit)
        {
            this.Home = home;
            this.OrderCollectionTimeLimit = orderCollectionTimeLimit;
        }

    }
}
