using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication;
using Common.Commands;

namespace Server.CmdHandler
{
    class CmdRegisterCustomerHandler : CommandHandler<CmdRegisterCustomer>
    {

        protected override void Handle(CmdRegisterCustomer command, string connectionIdOrNull)
        {
            Console.WriteLine("Yippie");   
        }
    }
}
