using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;

namespace Server.DriverController
{
    interface IDriverController
    {
        /// <summary>
        /// Returns the closest driver that can reach the destination until the end of his shift.
        /// </summary>
        /// <param name="allDrivers"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        Driver DetermineDriverOrNull(IEnumerable<Driver> allDrivers, IEnumerable<Order> allUnfinishedOrders, Address destination);

        /// <summary>
        /// Returns the closest driver that can reach the destination until the end of his shift.
        /// </summary>
        /// <param name="allDrivers"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        Driver DetermineDriverOrNull(IEnumerable<Driver> allDrivers, IEnumerable<Order> allUnfinishedOrders, GPSPosition destination);
    }
}
