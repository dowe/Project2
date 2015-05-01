using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnRegisterCustomer : ResponseCommand
    {

        public bool Success { get; private set; }

        public string Error { get; private set; }

        public CmdReturnRegisterCustomer(Guid requestId, bool success, string error) : base(requestId)
        {
            Success = success;
            Error = error;
        }


    }
}
