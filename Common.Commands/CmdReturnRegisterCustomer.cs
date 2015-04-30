using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnRegisterCustomer : Command
    {

        public bool Success { get; private set; }

        public string Error { get; private set; }

        public CmdReturnRegisterCustomer(bool success, string error)
        {
            Success = success;
            Error = error;
        }


    }
}
