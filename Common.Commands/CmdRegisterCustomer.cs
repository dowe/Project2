using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.DataTransferObjects;

namespace Common.Commands
{
    public class CmdRegisterCustomer : Command
    {

        public Customer Customer { get; private set; }

        public CmdRegisterCustomer(Customer customer)
        {
            Customer = customer;
        }

    }
}
