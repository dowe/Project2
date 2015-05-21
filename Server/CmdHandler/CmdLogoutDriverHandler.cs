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
    class CmdLogoutDriverHandler : CommandHandler<CmdLogoutDriver>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;
        private UsernameToConnectionIdMapping driverMapping = null;

        public CmdLogoutDriverHandler(IServerConnection connection, IDatabaseCommunicator db, UsernameToConnectionIdMapping driverMapping)
        {
            this.connection = connection;
            this.db = db;
            this.driverMapping = driverMapping;
        }

        protected override void Handle(CmdLogoutDriver command, string connectionIdOrNull)
        {
            bool success = true;

            db.StartTransaction();
            Car car = db.GetCar(command.CarId);
            if (car != null)
            {
                CarLogbookEntry latestEntry = car.CarLogbook.CarLogbookEntry.LastOrDefault();
                if (latestEntry != null)
                {
                    latestEntry.EndDateOrNull = DateTime.Now;
                    latestEntry.EndKMOrNull = command.EndKm;
                }
                if (car.CurrentDriver != null && car.CurrentDriver.UserName.Equals(command.Username))
                {
                    car.CurrentDriver = null;
                }
            }
            else
            {
                success = false;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);
            driverMapping.Remove(command.Username);
            CmdReturnLogoutDriver response = new CmdReturnLogoutDriver(command.Id, success);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
