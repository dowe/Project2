using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdGetBillsOfMonth : Command
    {

        public string Username { get; private set; }

        public CmdGetBillsOfMonth(string username)
        {
            Username = username;
        }

    }
}
