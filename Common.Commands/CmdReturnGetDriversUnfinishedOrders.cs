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
    public class CmdReturnGetDriversUnfinishedOrders : ResponseCommand
    {

        public ReadOnlyCollection<Order> UnfinishedOrders { get; private set; }

        public CmdReturnGetDriversUnfinishedOrders(Guid requestId, List<Order> unfinishedOrders) : base(requestId)
        {
            UnfinishedOrders = unfinishedOrders.AsReadOnly();
        }

    }
}
