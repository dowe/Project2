using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetAllOrders : ResponseCommand
    {

        public ReadOnlyCollection<Order> Orders { get; private set; }

        public CmdReturnGetAllOrders(Guid requestId, List<Order> orders) : base(requestId)
        {
            Orders = orders.AsReadOnly();
        }

    }
}
