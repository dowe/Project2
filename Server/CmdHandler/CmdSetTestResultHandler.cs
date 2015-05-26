using Common.Commands;
using Common.Communication;
using Common.DataTransferObjects;
using Common.Util;
using Server.DatabaseCommunication;
using Server.ExtremeValueCheck;
using Server.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Timer;
using Common.Communication.Server;

namespace Server.CmdHandler
{
    public class CmdSetTestResultHandler : CommandHandler<CmdSetTestResult>
    {
        private IDatabaseCommunicator db;
        private IExtremeValueChecker checker;
        private ISmsSending sms;
        private LocalServerData data;
        private IServerConnection connection;
        private ITimerFactory timerFactory;

        public CmdSetTestResultHandler(
            IDatabaseCommunicator db,
            IExtremeValueChecker checker,
            ISmsSending sms,
            LocalServerData data,
            IServerConnection connection,
            ITimerFactory timerFactory)
        {
            this.timerFactory = timerFactory;
            this.checker = checker;
            this.db = db;
            this.sms = sms;
            this.data = data;
            this.connection = connection;
        }

        protected override void Handle(CmdSetTestResult command, string connectionIdOrNull)
        {
            //PF 20/30/40
            db.StartTransaction();
            Test t = db.GetTest(command.TestId);
            Order o = db.GetAllOrders(x => ContainsTest(x, t)).FirstOrDefault<Order>();

            t.ResultValue = command.Result;
            t.EndDate = DateTime.Now;
            t.TestState = TestState.COMPLETED;
            t.Critical = checker.isExtreme(t);

            if (t.Critical)
            {
                t.AlarmState = AlarmState.FIRST_ALARM_TRANSMITTED;
                sms.Send(o.Customer.MobileNumber,
                  "1. Alarm-Meldung: "
                + "Das Testresultat des Test [" + t.Analysis.Name + "] "
                + "von Patient [" + t.PatientID + "], "
                + "der Bestellung [" + o.OrderID + "] "
                + "überschritt die Grenzwerte. "
                + "Zur Bestätigung dieses Alarms senden Sie diese SMS an die Sendenummer zurück.");

                Command cmdInject = new CmdCheckAlarmTenMinutesLeft(t.TestID, o.OrderID);
                InjectInternalTimed timer = TimerFactorys.InjectCmdTimer(connection, timerFactory, ()=>cmdInject);
                data.TimerList.Add(t.TestID, timer);
                timer.Start();
            }

            if (o.Customer.SMSRequested)
            {
                if (AllTestOfSampleOfPatientCompleted(o, t.PatientID, t.Analysis.SampleType))
                {
                    sms.Send(o.Customer.MobileNumber,
                      "Alle Tests zur Probe [" + SampleType(t.Analysis.SampleType) + "] "
                    + "von Patient [" + t.PatientID + "], "
                    + "der Bestellung [" + o.OrderID + "] "
                    + "sind abgeschlossen.");
                }
            }

            Test notCompleted = o.Test.Where<Test>((x) => x.TestState != TestState.COMPLETED).FirstOrDefault<Test>();
            if (notCompleted == null)
            {
                o.CompleteDate = DateTime.Now;
            }

            db.EndTransaction(TransactionEndOperation.SAVE);
        }

        private string SampleType(SampleType sampleType)
        {
            return Util.CreateValuePair<SampleType>(sampleType).Value;
        }

        private bool AllTestOfSampleOfPatientCompleted(Order o, string patientID, SampleType sampleType)
        {
            foreach (Test item in o.Test)
            {
                if (item.PatientID.Equals(patientID))
                {
                    if (item.Analysis.SampleType == sampleType)
                    {
                        if (item.TestState == TestState.COMPLETED)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool ContainsTest(Order o, Test t)
        {
            foreach (Test item in o.Test)
            {
                if (item.TestID == t.TestID)
                {
                    return true;
                }
            }
            return false;
        }



    }
}
