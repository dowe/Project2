using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetShiftSchedule : Command
    {

        public ReadOnlyCollection<ShiftSchedule> Schedules { get; private set; }

        public CmdReturnGetShiftSchedule(List<ShiftSchedule> schedules)
        {
            Schedules = schedules.AsReadOnly();
        }

    }
}
