using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdSetOrderCollected : Command
    {

        public long OrderId { get; private set; }

        public CmdSetOrderCollected(long orderId)
        {
            OrderId = orderId;
        }

    }
}
