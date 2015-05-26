using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Timer
{
    public interface ITimer
    {
        void Start(Action _DoActionScheduled);
        void Cancel();
    }
}
