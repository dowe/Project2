using Common.Communication;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class CmdReturnGetAllCustomers : ResponseCommand
    {

        public ReadOnlyCollection<Customer> Customers { get; private set; }

        public CmdReturnGetAllCustomers(Guid requestId, IList<Customer> customers)
            : base(requestId)
        {
            Customers = new ReadOnlyCollection<Customer>(customers);
        }

    }

}
