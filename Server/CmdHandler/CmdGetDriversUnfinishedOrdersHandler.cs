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
    class CmdGetDriversUnfinishedOrdersHandler : CommandHandler<CmdGetDriversUnfinishedOrders>
    {

        private IServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetDriversUnfinishedOrdersHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetDriversUnfinishedOrders command, string connectionIdOrNull)
        {
            db.StartTransaction();
            List<Order> driversUnfinishedOrders =
                db.GetAllOrders(o => o.Driver.UserName.Equals(command.Username) && o.CollectDate == null);
            CmdReturnGetDriversUnfinishedOrders response = new CmdReturnGetDriversUnfinishedOrders(command.Id,
                driversUnfinishedOrders);

            connection.Unicast(response, connectionIdOrNull);

            db.EndTransaction(TransactionEndOperation.READONLY);
        }
    }
}
