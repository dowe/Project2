using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Server;

namespace Server.CmdHandler
{
    class CmdLogoutDriverHandler : CommandHandler<CmdLogoutDriver>
    {

        private IServerConnection connection = null;

        public CmdLogoutDriverHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdLogoutDriver command, string connectionIdOrNull)
        {
            bool success = true;
            CmdReturnLogoutDriver response = new CmdReturnLogoutDriver(command.Id, success);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
