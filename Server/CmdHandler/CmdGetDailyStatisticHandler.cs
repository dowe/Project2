﻿using Common.Commands;
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
        private ILocalServerData data;
        DailyStatistic ds;
        public CmdGetDailyStatisticHandler(
            ServerConnection connection, 
            IDatabaseCommunicator db,
            ILocalServerData data)
        {
            this.connection = connection;
            this.db = db;
            this.data = data;
        }

        protected override void Handle(CmdGetDailyStatistic command, string connectionIdOrNull)
        {
            ds = data.DailyStatistic;
            ResponseCommand response = new CmdReturnGetDailyStatistic(command.Id, ds);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
