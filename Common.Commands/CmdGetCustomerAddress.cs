using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;

namespace Common.Commands
{
    public class CmdGetCustomerAddress : Command
    {
        public CmdGetCustomerAddress(string customerUsername)
        {
            CustomerUsername = customerUsername;
        }
        public string CustomerUsername
        {
            get;
            set;
        }
    }
}
