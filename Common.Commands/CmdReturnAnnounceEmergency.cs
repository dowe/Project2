﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnAnnounceEmergency : Command
    {

        public bool Success { get; private set; }

        public CmdReturnAnnounceEmergency(bool success)
        {
            Success = success;
        }

}
}
