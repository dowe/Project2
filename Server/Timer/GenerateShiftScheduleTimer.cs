using Common.Commands;
using Common.Communication.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    public class GenerateShiftScheduleTimer : ScheduledTimer
    {
        private IServerConnection connection;

        public GenerateShiftScheduleTimer(IServerConnection connection)
        {
            this.connection = connection;

            Start(GetTimeSpan, TimeElapsed);
        }

        private void TimeElapsed()
        {
            connection.InjectInternal(new CmdGenerateShiftSchedule());
        }

        private TimeSpan GetTimeSpan()
        {
            return TimeSpan.FromDays(1.0);
        }
    }


}
