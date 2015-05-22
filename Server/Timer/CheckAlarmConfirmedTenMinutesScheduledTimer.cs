using Common.Commands;
using Common.Communication.Server;
using Common.DataTransferObjects;
using Server.CmdHandler;
using Server.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    class CheckAlarmConfirmedTenMinutesScheduledTimer : ScheduledTimer
    {
          private IServerConnection connection = null;
          private Test test;
          private Order order;
          public CheckAlarmConfirmedTenMinutesScheduledTimer(IServerConnection connection, Test test, Order order)
        {
            this.test = test;
            this.order = order;
            this.connection = connection;
            Start(GetTimeSpan, DoAction, true);
        }

        private TimeSpan GetTimeSpan()
        {
            return TimeSpan.FromMinutes(10);
        }

        private void DoAction()
        {
            connection.InjectInternal(new CmdCheckAlarmTenMinutesLeft(test, order));
            
        }
    }
}
