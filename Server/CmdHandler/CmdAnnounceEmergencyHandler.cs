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
    class CmdAnnounceEmergencyHandler : CommandHandler<CmdAnnounceEmergency>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdAnnounceEmergencyHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdAnnounceEmergency command, string connectionIdOrNull)
        {
            bool success = true;

            db.StartTransaction();
            Car car = db.GetCar(command.CarID);
            if (car != null)
            {
                car.Roadworthy = false;
                CarLogbookEntry latestEntry = car.CarLogbook.CarLogbookEntry.LastOrDefault();
                if (latestEntry != null)
                {
                    latestEntry.EndDateOrNull = DateTime.Now;
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
            // TODO Find driver that continues collecting the unfinished orders.

            CmdReturnAnnounceEmergency response = new CmdReturnAnnounceEmergency(command.Id, success);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
