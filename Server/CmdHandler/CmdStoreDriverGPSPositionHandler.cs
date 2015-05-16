using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;

namespace Server.CmdHandler
{
    class CmdStoreDriverGPSPositionHandler : CommandHandler<CmdStoreDriverGPSPosition>
    {
        protected override void Handle(CmdStoreDriverGPSPosition command, string connectionIdOrNull)
        {
            Console.WriteLine("Car " + command.CarID + " at position " + command.DriverGPSPosition.Latitude + ", " +
                              command.DriverGPSPosition.Longitude);
        }
    }
}
