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
    public class CmdReturnGetUsersOrderResults : ResponseCommand
    {

        public ReadOnlyCollection<Order> Orders { get; private set; }

        public CmdReturnGetUsersOrderResults(Guid requestId, IList<Order> orders) : base(requestId)
        {
            Orders = new ReadOnlyCollection<Order>(orders);
        }

    }
}
