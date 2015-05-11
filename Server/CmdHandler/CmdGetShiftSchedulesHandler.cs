using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Server.DatabaseCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.CmdHandler
{
    public class CmdGetShiftSchedulesHandler : CommandHandler<CmdGetShiftSchedules>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;

        public CmdGetShiftSchedulesHandler(
            IServerConnection connection, 
            IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
        }

        protected override void Handle(CmdGetShiftSchedules command, string connectionIdOrNull)
        {
            //TODO
        }
    }
}
