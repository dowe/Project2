﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdAnnounceEmergency : Command
    {

        public string Username { get; private set; }
        public string CarID { get; private set; }
        public GPSPosition DriverGPSPosition { get; private set; }

        public CmdAnnounceEmergency(string username, string carID, GPSPosition driverGPSPosition)
        {
            Username = username;
            CarID = carID;
            DriverGPSPosition = driverGPSPosition;
        }

    }
}
