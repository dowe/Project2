using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication.Server;

namespace Server.Timer
{
    class CheckOrdersFiveHoursLeftScheduledTimer : ScheduledTimer
    {

        private IServerConnection connection = null;

        public CheckOrdersFiveHoursLeftScheduledTimer(IServerConnection connection)
        {
            this.connection = connection;
            Start(GetTimeSpan, DoAction, false);
        }

        private TimeSpan GetTimeSpan()
        {
            return TimeSpan.FromHours(1);
        }

        private void DoAction()
        {
            connection.InjectInternal(new CmdCheckOrdersFiveHoursLeft());
        }

    }
}
