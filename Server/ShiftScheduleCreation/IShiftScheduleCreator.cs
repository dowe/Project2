using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;

namespace Server.ShiftScheduleCreation
{
    public interface IShiftScheduleCreator
    {
        ShiftSchedule createShiftSchedule(ShiftSchedule last, DateTime date);
    }
}
