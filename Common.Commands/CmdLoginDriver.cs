using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdLoginDriver : Command
    {

        public string Username { get; private set; }
        public string Password { get; private set; }

        public CmdLoginDriver(string username, string password, float startKm)
        {
            Username = username;
            Password = password;
        }

    }
}
