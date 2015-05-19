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
    class CmdGetUsersOrderResultsHandler : CommandHandler<CmdGetUsersOrderResults>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdGetUsersOrderResultsHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetUsersOrderResults command, string connectionIdOrNull)
        {
            IList<Order> userOrders;

            // Get all orders from the customer that is currently logged in
            db.StartTransaction();
            userOrders = db.GetAllOrders(order => order.Customer.UserName == command.Username);

            var response = new CmdReturnGetUsersOrderResults(command.Id, userOrders);
            connection.Unicast(response, connectionIdOrNull);
            db.EndTransaction(TransactionEndOperation.READONLY);
        }
    }
}
