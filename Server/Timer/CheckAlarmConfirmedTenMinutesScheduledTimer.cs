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
    public class CheckAlarmConfirmedTenMinutesScheduledTimer : ScheduledTimer
    {
          private IServerConnection connection = null;
          private Guid testID;
          private long orderID;
          private LocalServerData data;

          public CheckAlarmConfirmedTenMinutesScheduledTimer(
              IServerConnection connection, 
              LocalServerData data, 
              Guid testID,
              long orderID)
        {
            this.data = data;
            this.testID = testID;
            this.orderID = orderID;
            this.connection = connection;

            data.TimerList.Add(testID, this);

            Start(GetTimeSpan, DoAction, true);
        }

        private TimeSpan GetTimeSpan()
        {
            return TimeSpan.FromMinutes(10);
        }

        private void DoAction()
        {
            connection.InjectInternal(new CmdCheckAlarmTenMinutesLeft(testID, orderID));
        }
    }
}
