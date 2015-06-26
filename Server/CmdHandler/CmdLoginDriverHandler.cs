using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;

namespace Server.CmdHandler
{
    class CmdLoginDriverHandler : CommandHandler<CmdLoginDriver>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;
        private UsernameToConnectionIdMapping driverMapping = null;
        public CmdLoginDriverHandler(IServerConnection connection, IDatabaseCommunicator db, UsernameToConnectionIdMapping driverMapping)
        {
            this.connection = connection;
            this.db = db;
            this.driverMapping = driverMapping;
        }

        protected override void Handle(CmdLoginDriver command, string connectionIdOrNull)
        {
            db.StartTransaction();
            Car assignedCarOrNull = db.GetAllCars(c => c.CurrentDriver != null && c.CurrentDriver.UserName.Equals(command.Username))
                    .FirstOrDefault();
            Driver driverOrNull = null;
            string assignedCarIDOrNull = null;
            if (assignedCarOrNull != null)
            {
                // Driver already has a car assigned.
                driverOrNull = assignedCarOrNull.CurrentDriver;
                assignedCarIDOrNull = assignedCarOrNull.CarID;
            }
            else
            {
                // Driver does not have a car assigned.
                driverOrNull = db.GetDriver(command.Username);
                assignedCarIDOrNull = null;
            }

            // Check credentials.
            bool success = false;
            if (driverOrNull != null && driverOrNull.Password.Equals(command.Password))
            {
                // Logout existing session if existing.
                string oldSessionConnectionStringOrNull = driverMapping.ResolveConnectionIDOrNull(driverOrNull.UserName);
                if (oldSessionConnectionStringOrNull != null)
                {
                    var cmdForceDriverLogout = new CmdForceDriverLogout();
                    connection.Unicast(cmdForceDriverLogout, oldSessionConnectionStringOrNull);
                }

                driverMapping.Set(driverOrNull.UserName, connectionIdOrNull);
                success = true;
            }
            else
            {
                success = false;
                assignedCarIDOrNull = null;
            }

            var response = new CmdReturnLoginDriver(command.Id, assignedCarIDOrNull, success);
            connection.Unicast(response, connectionIdOrNull);

            db.EndTransaction(TransactionEndOperation.READONLY);
        }

    }
}
