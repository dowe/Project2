using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdGetBillDatesOfMonth : Command
    {

        public string Username { get; private set; }

        public CmdGetBillDatesOfMonth(string username)
        {
            Username = username;
        }

    }
}
