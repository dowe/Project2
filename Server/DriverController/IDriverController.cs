using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;

namespace Server.DriverController
{
    public interface IDriverController
    {
        /// <summary>
        /// Returns the driver that can reach the destination under all constraints in the shortest time.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        Driver DetermineDriverOrNullInsideTransaction(IDatabaseCommunicator db, GPSPosition position);

        /// <summary>
        /// Returns the driver that can reach the destination under all constraints in the shortest time.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        Driver DetermineDriverOrNullInsideTransaction(IDatabaseCommunicator db, Address address);
    }
}
