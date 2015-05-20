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
    public class CmdGetAllBillsOfUserHandler : CommandHandler<CmdGetAllBillsOfUser>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db;

        public CmdGetAllBillsOfUserHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetAllBillsOfUser command, string connectionIdOrNull)
        {
            db.StartTransaction();
            var billsRaw = db.GetAllBills(b => b.Customer.UserName == command.Username);
           

            var response = new CmdReturnGetAllBillsOfUser(command.Id, billsRaw);
            connection.Unicast(response, connectionIdOrNull);
            db.EndTransaction(TransactionEndOperation.READONLY);
        }
    }
}
