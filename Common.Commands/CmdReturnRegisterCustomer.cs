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

        public string Message { get; private set; }

        public CmdReturnRegisterCustomer(Guid requestId, bool success, string message)
            : base(requestId)
        {
            Success = success;
            Message = message;
        }


    }
}
