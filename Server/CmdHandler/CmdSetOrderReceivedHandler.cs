using Common.Commands;
using Common.Communication;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CmdHandler
{
    public class CmdSetOrderReceivedHandler : CommandHandler<CmdSetOrderReceived>
    {
        private IDatabaseCommunicator db;

        public CmdSetOrderReceivedHandler(IDatabaseCommunicator db)
        {
            this.db = db;
        }

        protected override void Handle(CmdSetOrderReceived command, string connectionIdOrNull)
        {
            db.StartTransaction();
            Order order = db.GetOrder(command.OrderId);

            order.BringDate = DateTime.Now;
            if (order.Driver == null)
            {
                order.CollectDate = DateTime.Now;
            }
            foreach ( Test t in order.Test ) {
                t.StartDate = DateTime.Now;
                t.TestState = TestState.IN_PROGRESS;
            }
            db.EndTransaction(TransactionEndOperation.SAVE);
        }
    }
}
