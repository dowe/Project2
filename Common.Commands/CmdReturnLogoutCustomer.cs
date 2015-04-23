using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnLogoutCustomer : Command
    {

        public bool Success { get; private set; }

        public CmdReturnLogoutCustomer(bool success)
        {
            Success = success;
        }

    }
}
