using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    public class InjectInternalTimeds
    {
        public static InjectInternalTimed InjectCmdTimer(
            IServerConnection connection,
            ITimerFactory timerFactory,
            Func<Command> command)
        {
            return new InjectInternalTimed(connection, timerFactory.CreateInstance(), command);
        }

        public static InjectInternalTimed GenerateShiftScheduleTimer(IServerConnection connection)
        {
            return InjectCmdTimer(connection, TimerFactorys.Daily(), () => new CmdGenerateShiftSchedule());
        }

        public static InjectInternalTimed CheckOrdersFiveHoursLeftScheduledTimer(IServerConnection connection)
        {
            return InjectCmdTimer(connection, TimerFactorys.Hourly(), () => new CmdCheckOrdersFiveHoursLeft());
        }
    }
}
