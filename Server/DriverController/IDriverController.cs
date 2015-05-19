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
        /// Returns the driver that can reach the destination under all constraints in the shortest time.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        Driver DetermineDriverOrNull(IDatabaseCommunicator db, GPSPosition position);

        /// <summary>
        /// Returns the driver that can reach the destination under all constraints in the shortest time.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        Driver DetermineDriverOrNull(IDatabaseCommunicator db, Address address);
    }
}
