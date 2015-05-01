using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdReturnAddOrder : ResponseCommand
    {

        public long CreatedOrderId { get; private set; }

        public CmdReturnAddOrder(Guid requestId, long createdOrderId) : base(requestId)
        {
            CreatedOrderId = createdOrderId;
        }

    }
}
