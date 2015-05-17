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
    public class CmdGetDailyStatisticHandler : CommandHandler<CmdGetDailyStatistic>
    {
       private ServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetDailyStatisticHandler(
            ServerConnection connection, 
            IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetDailyStatistic command, string connectionIdOrNull)
        {
            DailyStatistic ds = new DailyStatistic();
          
            db.StartTransaction();
            
            db.EndTransaction(TransactionEndOperation.READONLY);

   

            ResponseCommand response = new CmdReturnGetDailyStatistic(command.Id, ds);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
