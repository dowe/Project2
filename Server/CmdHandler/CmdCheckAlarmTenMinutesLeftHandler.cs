using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.DatabaseCommunication;
using Server.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.CmdHandler
{
    public class CmdCheckAlarmTenMinutesLeftHandler : CommandHandler<CmdCheckAlarmTenMinutesLeft>
    {
        private IServerConnection connection = null;
        private IDatabaseCommunicator db = null;
        private ISmsSending sms;
        private LocalServerData data;

        public CmdCheckAlarmTenMinutesLeftHandler(
            IServerConnection connection,
            IDatabaseCommunicator db,
            ISmsSending sms,
            LocalServerData data)
        {
            this.connection = connection;
            this.db = db;
            this.sms = sms;
            this.data = data;
        }

        protected override void Handle(CmdCheckAlarmTenMinutesLeft command, string connectionIdOrNull)
        {
            //PF 50
            if (data.TimerList.Remove(command.TestID))
            {
                db.StartTransaction();
                Test test = db.GetTest(command.TestID);
                Order order = db.GetOrder(command.OrderID);
                test.AlarmState = AlarmState.SECOND_ALARM_TRANSMITTED;
                sms.Send(order.Customer.MobileNumber, "2. Alarm-Meldung: Das Testresultat des Test " + test.Analysis.Name + " des Patienten " + test.PatientID + " der Bestellung" + command.OrderID + " überschritt die Grenzwerte.");
                db.EndTransaction(TransactionEndOperation.SAVE);
            }
        }
    }
}
