using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Communication;
using Common.Commands;
using Common.Communication.Server;

namespace Server.CmdHandler
{
    class CmdRegisterCustomerHandler : CommandHandler<CmdRegisterCustomer>
    {
        private IServerConnection connection;

        public CmdRegisterCustomerHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdRegisterCustomer command, string connectionIdOrNull)
        {
            
            //TODO: WRITE HANDLER

            CmdReturnRegisterCustomer response = new CmdReturnRegisterCustomer(command.Id, false, command.Customer.FirstName);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
