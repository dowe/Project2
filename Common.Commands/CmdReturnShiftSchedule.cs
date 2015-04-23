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
    public class CmdReturnShiftSchedule : Command
    {

        public ReadOnlyCollection<ShiftSchedule> Schedules { get; private set; }

        public CmdReturnShiftSchedule(List<ShiftSchedule> schedules)
        {
            Schedules = schedules.AsReadOnly();
        }

    }
}
