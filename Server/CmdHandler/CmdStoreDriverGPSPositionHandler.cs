using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;

namespace Server.CmdHandler
{
    class CmdStoreDriverGPSPositionHandler : CommandHandler<CmdStoreDriverGPSPosition>
    {

        private IDatabaseCommunicator db = null;

        public CmdStoreDriverGPSPositionHandler(IDatabaseCommunicator db)
        {
            this.db = db;
        }

        protected override void Handle(CmdStoreDriverGPSPosition command, string connectionIdOrNull)
        {
            db.StartTransaction();
            GPSPosition lastPosition = db.GetGPSPosition(command.CarID);
            if (lastPosition != null)
            {
                lastPosition.Latitude = command.DriverGPSPosition.Latitude;
                lastPosition.Longitude = command.DriverGPSPosition.Longitude;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);
        }
    }
}
