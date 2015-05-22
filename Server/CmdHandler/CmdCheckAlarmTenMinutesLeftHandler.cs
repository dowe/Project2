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
        Test test;
        SmsSending sms;

        public CmdCheckAlarmTenMinutesLeftHandler(IServerConnection connection, IDatabaseCommunicator db)
        {
            this.connection = connection;
            this.db = db;
            sms = new SmsSending();
        }

        

        protected override void Handle(CmdCheckAlarmTenMinutesLeft command, string connectionIdOrNull)
        {
            db.StartTransaction();
            test = db.GetTest(command.Test.TestID);
            test.AlarmState = AlarmState.SECOND_ALARM_TRANSMITTED;
            sms.Send(command.Order.Customer.MobileNumber, "2. Alarm-Meldung: Das Testresultat des Test " + test.Analysis.Name +" des Patienten " + test.PatientID + " der Bestellung"+ command.Order.OrderID + " überschritt die Grenzwerte.");
            db.EndTransaction(TransactionEndOperation.SAVE);

        }
    }
}
