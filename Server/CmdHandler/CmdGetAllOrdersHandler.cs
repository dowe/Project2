using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CmdHandler
{
    public class CmdGetAllOrdersHandler : CommandHandler<CmdGetAllOrders>
    {

          private ServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetAllOrdersHandler(
            ServerConnection connection, 
            IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }
        

        protected override void Handle(CmdGetAllOrders command, string connectionIdOrNull)
        {
           
            db.StartTransaction();
            IList<Order> list = db.GetAllOrders(null);
            ResponseCommand response = new CmdReturnGetAllOrders(command.Id, list);
            connection.Unicast(response, connectionIdOrNull);
            db.EndTransaction(TransactionEndOperation.READONLY);

        }
    }
}
