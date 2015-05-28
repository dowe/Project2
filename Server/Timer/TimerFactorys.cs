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
        public static ITimerFactory TenMinuteOnceForSecondAlarmTransmmit()
        {
            return new TimerFactory(() => TimeSpan.FromMinutes(10.0), true);
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
            return InjectCmdTimer(connection, new TimerFactory(() => TimeSpan.FromDays(1.0), false), () => new CmdGenerateShiftSchedule());
        }

        public static InjectInternalTimed CheckOrdersFiveHoursLeftScheduledTimer(IServerConnection connection)
        {
            return InjectCmdTimer(connection, new TimerFactory(() => TimeSpan.FromHours(1.0), false), () => new CmdCheckOrdersFiveHoursLeft());
        }
    }
}
