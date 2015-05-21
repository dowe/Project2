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
    class CmdSetOrderCollectedHandler : CommandHandler<CmdSetOrderCollected>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;

        public CmdSetOrderCollectedHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdSetOrderCollected command, string connectionIdOrNull)
        {
            bool success = true;

            db.StartTransaction();
            Order order = db.GetOrder(command.OrderId);
            if (order != null && order.CollectDate == null)
            {
                order.CollectDate = DateTime.Now;
                foreach (Test test in order.Test)
                {
                    test.TestState = TestState.WAITING_FOR_DRIVER;
                }
            }
            else
            {
                success = false;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);

            CmdReturnSetOrderCollected response = new CmdReturnSetOrderCollected(command.Id, command.OrderId, success);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
