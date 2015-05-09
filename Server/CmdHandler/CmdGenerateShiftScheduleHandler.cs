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
    public class CmdGenerateShiftScheduleHandler : CommandHandler<CmdGenerateShiftSchedule>
    {
        private IServerConnection connection;
        private IDatabaseCommunicator db;
        private LocalServerData data;

        public CmdGenerateShiftScheduleHandler(
            IServerConnection connection,
            IDatabaseCommunicator db, 
            LocalServerData data)
        {
            this.connection = connection;
            this.db = db;
            this.data = data;

            //TODO create Timer and store in LocalServerData
        }

        protected override void Handle(CmdGenerateShiftSchedule command, string connectionIdOrNull)
        {
            throw new NotImplementedException();

            //TODO create ShiftSchedule
            //TODO store ShiftSchedule in db
        }
    }
}
