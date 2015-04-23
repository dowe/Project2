using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdStoreDriverGPSPosition : Command
    {

        public string Username { get; private set; }
        public GPSPosition DriverGPSPosition { get; private set; }

        public CmdStoreDriverGPSPosition(string username, GPSPosition driverGPSPosition)
        {
            Username = username;
            DriverGPSPosition = driverGPSPosition;
        }

    }
}
