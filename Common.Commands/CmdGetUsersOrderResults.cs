using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdGetUsersOrderResults : Command
    {

        public string Username { get; private set; }

        public CmdGetUsersOrderResults(string username)
        {
            Username = username;
        }

    }
}
