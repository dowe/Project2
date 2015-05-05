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
    class CmdLoginDriverHandler : CommandHandler<CmdLoginDriver>
    {

        private IServerConnection connection = null;

        public CmdLoginDriverHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdLoginDriver command, string connectionIdOrNull)
        {
            bool success = false;
            if ("Ole".Equals(command.Username) && "o".Equals(command.Password))
            {
                success = true;
            }
            else
            {
                success = false;
            }

            var response = new CmdReturnLoginDriver(command.Id, success);
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
