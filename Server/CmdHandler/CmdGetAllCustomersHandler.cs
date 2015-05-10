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
    public class CmdGetAllCustomersHandler : CommandHandler<CmdGetAllCustomers>
    {
        private ServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetAllCustomersHandler(
            ServerConnection connection, 
            IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }
        

        protected override void Handle(CmdGetAllCustomers command, string connectionIdOrNull)
        {
            IList<Customer> list;

            db.StartTransaction();
            list = db.GetAllCustomer(null);
            db.EndTransaction(TransactionEndOperation.READONLY);

            ResponseCommand response = new CmdReturnGetAllCustomers(command.Id, list);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
