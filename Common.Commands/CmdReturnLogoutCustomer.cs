using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnLogoutCustomer : ResponseCommand
    {

        public bool Success { get; private set; }

        public CmdReturnLogoutCustomer(Guid requestId, bool success) : base(requestId)
        {
            Success = success;
        }

    }
}
