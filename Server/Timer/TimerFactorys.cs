using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
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
    }
}
