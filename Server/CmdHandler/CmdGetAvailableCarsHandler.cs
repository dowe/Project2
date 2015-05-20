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
    class CmdGetAvailableCarsHandler : CommandHandler<CmdGetAvailableCars>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdGetAvailableCarsHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetAvailableCars command, string connectionIdOrNull)
        {
            db.StartTransaction();
            List<Car> cars = db.GetAllCars(c => c.CurrentDriver == null);

            CmdReturnGetAvailableCars response = new CmdReturnGetAvailableCars(command.Id, cars);

            connection.Unicast(response, connectionIdOrNull);

            db.EndTransaction(TransactionEndOperation.READONLY);
        }
    }
}
