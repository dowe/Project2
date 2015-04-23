using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdUpdateOrder : Command
    {
        public Order Order { get; private set; }

        public CmdUpdateOrder(Order order)
        {
            Order = order;
        }
    }
}
