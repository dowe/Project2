using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdReturnGetCustomerAddress : Command
    {

        public Address CustomerAddress { get; private set; }

        public CmdReturnGetCustomerAddress(Address customerAddress)
        {
            CustomerAddress = customerAddress;
        }

    }
}
