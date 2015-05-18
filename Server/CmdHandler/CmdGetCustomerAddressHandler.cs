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
    public class CmdGetCustomerAddressHandler : CommandHandler<CmdGetCustomerAddress>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db;

        public CmdGetCustomerAddressHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetCustomerAddress request, string connectionIdOrNull)
        {
            Address address = null;
            db.StartTransaction();
            Customer c = db.GetCustomer(request.CustomerUsername);
            if ( c != null ) {
                address = c.Address;
            }
            ResponseCommand response = new CmdReturnGetCustomerAddress(request.Id, address);
            connection.Unicast(response, connectionIdOrNull);
            db.EndTransaction(TransactionEndOperation.READONLY);
        }
    }
}
