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
using Server.DriverControlling;

namespace Server.CmdHandler
{
    public class CmdAddOrderHandler : CommandHandler<CmdAddOrder>
    {

        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;
        private IDriverController driverController = null;
        private UsernameToConnectionIdMapping driverMap = null;

        public CmdAddOrderHandler(IServerConnection connection, IDatabaseCommunicator db, IDriverController driverController, UsernameToConnectionIdMapping driverMap)
        {
            this.connection = connection;
            this.db = db;
            this.driverController = driverController;
            this.driverMap = driverMap;
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

            if (optimalDriverOrNull != null)
            {
                Console.WriteLine("Assigned order to driver " + optimalDriverOrNull.UserName + ".");
                var sendNotification = new CmdSendNotification(order);
                string connectionId = driverMap.ResolveConnectionIDOrNull(optimalDriverOrNull.UserName);
                if(connectionId != null)
                {
                    connection.Unicast(sendNotification, connectionId);
                }
            }
            else
            {
                // TODO: Call taxi.
            }

            CmdReturnAddOrder ret = new CmdReturnAddOrder(command.Id, order.OrderID);
            connection.Unicast(ret, connectionIdOrNull);

            db.EndTransaction(TransactionEndOperation.SAVE);
        }
    }
}
