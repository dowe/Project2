using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    [ExcludeFromCodeCoverage]
    public class TimerFactorys
    {
        public static ITimerFactory TenMinuteOnce()
        {
            return new TimerFactory(() => TimeSpan.FromMinutes(10.0), true);
        }

        public static ITimerFactory Daily()
        {
            return new TimerFactory(() => TimeSpan.FromDays(1.0), false);
        }

        public static ITimerFactory Hourly()
        {
            return new TimerFactory(() => TimeSpan.FromHours(1.0), false);
        }

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
