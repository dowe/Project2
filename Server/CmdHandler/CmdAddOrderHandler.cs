using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using Server.DriverController;

namespace Server.CmdHandler
{
    public class CmdAddOrderHandler : CommandHandler<CmdAddOrder>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db;
        private IDriverController driverController = null;

        public CmdAddOrderHandler(IServerConnection connection, IDatabaseCommunicator db, IDriverController driverController)
        {
            this.connection = connection;
            this.db = db;
            this.driverController = driverController;
        }

        protected override void Handle(CmdAddOrder command, string connectionIdOrNull)
        {
            db.StartTransaction();
            Customer customer = db.GetCustomer(command.CustomerUsername);
            Driver optimalDriverOrNull = driverController.DetermineDriverOrNullInsideTransaction(db, customer.Address);
            foreach(KeyValuePair<String, List<Analysis>> kv in command.PatientTests)
            {
                db.AttachAnalysises(kv.Value);
            }
            Order order = new Order(command.PatientTests, db.GetCustomer(command.CustomerUsername))
            {
                Driver = optimalDriverOrNull
            };
            db.CreateOrder(order);
            db.EndTransaction(TransactionEndOperation.SAVE);

            if (optimalDriverOrNull != null)
            {
                // TODO: Notify driver.
            }
            else
            {
                // TODO: Call taxi.
            }

            CmdReturnAddOrder ret = new CmdReturnAddOrder(command.Id, order.OrderID);
            connection.Unicast(ret, connectionIdOrNull);
        }
    }
}
