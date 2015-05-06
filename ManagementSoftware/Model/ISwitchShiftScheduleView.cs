using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSoftware.Model
{
    public interface ISwitchShiftScheduleView
    {
        void SwitchToShiftScheduleMonthVM();
        void SwitchToShiftScheduleDayVM();
    }
}
