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
    class CmdLoginDriverHandler : CommandHandler<CmdLoginDriver>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdLoginDriverHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdLoginDriver command, string connectionIdOrNull)
        {
            bool success = false;

            db.StartTransaction();
            Driver driver = db.GetDriver(command.Username);
            db.EndTransaction(TransactionEndOperation.READONLY);

            if (driver != null && driver.Password.Equals(command.Password))
            {
                success = true;
            }
            else
            {
                success = false;
            }

            var response = new CmdReturnLoginDriver(command.Id, success);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
