using Common.Commands;
using Common.Communication.Server;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using Common.Communication;


namespace Server.CmdHandler
{
    public class CmdGenerateDailyStatisticHandler : CommandHandler<CmdGenerateDailyStatistic>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;
        private DailyStatistic ds;
        LocalServerData data;
        private IList<Order> _OrderList;
        public CmdGenerateDailyStatisticHandler(
            IServerConnection connection,
            IDatabaseCommunicator db,
            LocalServerData data)
        {
            this.connection = connection;
            this.db = db;
            this.data = data;
            ds = new DailyStatistic();
        }

        protected override void Handle(CmdGenerateDailyStatistic command, string connectionIdOrNull)
        {
          
            db.StartTransaction();
            _OrderList = db.GetAllOrders(null);
            db.EndTransaction(TransactionEndOperation.READONLY);

            foreach (Order o in _OrderList)
            {
                //Alles für Orders
                //falls in den letzten 24h bestellt wurde
                if (o.OrderDate != null && (o.OrderDate.GetValueOrDefault() - DateTime.Now).TotalHours < 24)
                    ds.NumberOfOrderedOrders++;
                //falls in den letzten 24h beendet wurde
                if (o.CompleteDate != null && (o.CompleteDate.GetValueOrDefault() - DateTime.Now).TotalHours < 24)
                    ds.NumberOfCompletedOrders++;
                //sonst falls in progress
                else if (o.BringDate != null && (o.BringDate.GetValueOrDefault() - DateTime.Now).TotalHours < 24)
                    ds.NumberOfOrdersInProgress++;

                //Alles für tests
                foreach (Test t in o.Test)
                {
                    //in letzten 24h gestartet aber noch nicht zu ende
                    if (t.StartDate != null && (t.StartDate.GetValueOrDefault() - DateTime.Now).TotalHours < 24 && t.EndDate == null)
                        ds.NumberOfTestsInProgress++;
                    //in letzten 24h beendet
                    else if (t.EndDate != null && (t.EndDate.GetValueOrDefault() - DateTime.Now).TotalHours < 24)
                        ds.NumberOfCompletedTests++;
                }

            }
            data.DailyStatistic = ds;


        }
    }
    
}
