using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;

namespace Server.CmdHandler
{
    class CmdLoginCustomerHandler : CommandHandler<CmdLoginCustomer>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdLoginCustomerHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdLoginCustomer command, string connectionIdOrNull)
        {
            bool success = false;
            db.StartTransaction();
            Customer customer = db.GetCustomer(command.Username);
            db.EndTransaction(TransactionEndOperation.READONLY);
            if (customer != null && command.Username == customer.UserName && command.Password == customer.Password)
                success = true;

            var response = new CmdReturnLoginCustomer(command.Id, success);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
