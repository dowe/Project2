using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdLoginCustomer : Command
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        public CmdLoginCustomer(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}
