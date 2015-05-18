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
    public class CmdAddOrderHandler : CommandHandler<CmdAddOrder>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db;

        public CmdAddOrderHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdAddOrder command, string connectionIdOrNull)
        {
            db.StartTransaction();
            foreach(KeyValuePair<String, List<Analysis>> kv in command.PatientTests)
            {
                db.AttachAnalysises(kv.Value);
            }
            Order order = new Order(command.PatientTests, db.GetCustomer(command.CustomerUsername));
            db.CreateOrder(order);
            db.EndTransaction(TransactionEndOperation.SAVE);
            CmdReturnAddOrder ret = new CmdReturnAddOrder(command.Id, order.OrderID);
            connection.Unicast(ret, connectionIdOrNull);
        }
    }
}
