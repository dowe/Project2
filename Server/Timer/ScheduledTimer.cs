using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Timer
{
    public class ScheduledTimer
    {
        private Func<TimeSpan> _GetSpan;
        private Action _DoActionScheduled;
        private CancellationTokenSource _ctSource;
        private bool once;

        protected void Start(Func<TimeSpan> _GetSpan, Action _DoActionScheduled, bool once)
        {
            this._GetSpan = _GetSpan;
            this._DoActionScheduled = _DoActionScheduled;
            this.once = once;
            RunCode();
        } 

        private void RunCode()
        {
            _ctSource = new CancellationTokenSource();
            TimeSpan ts = _GetSpan();

            //waits certan time and run the code, in meantime you can cancel the task at anty time
            Task.Delay(ts).ContinueWith((x) =>
                {
                    //run the code at the time
                    _DoActionScheduled();

                    //setup call next day
                    if (!once)
                    {
                        RunCode();
                    }

                }, _ctSource.Token);
        }

        public void Cancel()
        {
            if (_ctSource != null)
            {
                _ctSource.Cancel();
                _ctSource = null;
            }
        }

    }
}
