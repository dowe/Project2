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
    public class CmdReturnGetDriversUnfinishedOrders : Command
    {

        public ReadOnlyCollection<Order> UnfinishedOrders { get; private set; }

        public CmdReturnGetDriversUnfinishedOrders(List<Order> unfinishedOrders)
        {
            UnfinishedOrders = unfinishedOrders.AsReadOnly();
        }

    }
}
