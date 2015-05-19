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
    public class CmdGetAllOrdersHandler : CommandHandler<CmdGetAllOrders>
    {

          private ServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetAllOrdersHandler(
            ServerConnection connection, 
            IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }
        

        protected override void Handle(CmdGetAllOrders command, string connectionIdOrNull)
        {
           
            db.StartTransaction();
            IList<Order> list = db.GetAllOrders(null);
            db.EndTransaction(TransactionEndOperation.READONLY);

            //Start Creating a Mockorder for testing
            Order MockOrder = new Order();
            MockOrder.CollectDate = DateTime.Now;
            Customer MockCustomer = new Customer();
            MockCustomer.UserName = "myUsername";
            MockCustomer.LastName = "Müller";
            MockCustomer.FirstName = "hans";
            MockOrder.Customer = MockCustomer;
            MockOrder.OrderID = 1111;
            List<Test> MockTest = new List<Test>();
            MockTest.Add(new Test("Patientenid123", new Analysis("Blutzeug", 1.0f, 11.0f, "Kilo", 22.2f, SampleType.BLOOD)));
            MockOrder.Test = MockTest;
            list.Add(MockOrder);
            //finish Mockorder

            ResponseCommand response = new CmdReturnGetAllOrders(command.Id, list);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
