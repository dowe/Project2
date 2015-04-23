using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdAddOrder : Command
    {

        public Order Order { get; private set; }
        public string CustomerUsername { get; private set; }

        public CmdAddOrder(Order order, string customerUsername)
        {
            Order = order;
            CustomerUsername = customerUsername;
        }

    }
}
