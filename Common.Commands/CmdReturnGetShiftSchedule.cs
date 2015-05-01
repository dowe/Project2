﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetShiftSchedule : ResponseCommand
    {

        public ReadOnlyCollection<ShiftSchedule> Schedules { get; private set; }

        public CmdReturnGetShiftSchedule(Guid requestId, List<ShiftSchedule> schedules) : base(requestId)
        {
            Schedules = schedules.AsReadOnly();
        }

    }
}
