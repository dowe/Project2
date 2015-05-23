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
    public class CmdSetFirstAlertReceivedHandler : CommandHandler<CmdSetFirstAlertReceived>
    {
        private IDatabaseCommunicator db;
        private LocalServerData data;

        public CmdSetFirstAlertReceivedHandler(
            IDatabaseCommunicator db,
            LocalServerData data)
        {
            this.db = db;
            this.data = data;
        }


        protected override void Handle(CmdSetFirstAlertReceived command, string connectionIdOrNull)
        {
            //PF 220
            if (data.TimerList.Remove(command.TestId))
            {
                db.StartTransaction();
                db.GetTest(command.TestId).AlarmState = AlarmState.FIRST_ALARM_CONFIRMED;
                db.EndTransaction(TransactionEndOperation.SAVE);
            }
        }
    }
}
