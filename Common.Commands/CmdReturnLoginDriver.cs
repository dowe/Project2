using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnLoginDriver : ResponseCommand
    {

        public bool Success { get; private set; }
        public string AssignedCarIDOrNull { get; private set; }

        public CmdReturnLoginDriver(Guid requestId, string assignedCarIDOrNull, bool success) : base(requestId)
        {
            Success = success;
            AssignedCarIDOrNull = assignedCarIDOrNull;
        }

    }
}
