using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    public class TimerFactory : ITimerFactory
    {
        private readonly Func<TimeSpan> _GetSpan;
        private readonly bool _Once;

        public TimerFactory(
            Func<TimeSpan> _GetSpan,
            bool _Once)
        {
            this._GetSpan = _GetSpan;
            this._Once = _Once;
        }

        public ITimer CreateInstance()
        {
            return new ScheduledTimer(this._GetSpan, this._Once);
        }
    }
}
