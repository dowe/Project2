using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdLogoutDriver : Command
    {

        public string Username { get; private set; }
        public string CarId { get; private set; }
        public float EndKm { get; private set; }

        public CmdLogoutDriver(string username, string carId, float endKm)
        {
            Username = username;
            CarId = carId;
            EndKm = endKm;
        }

    }
}
