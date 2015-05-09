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
    class CmdSelectCarHandler : CommandHandler<CmdSelectCar>
    {

        private IServerConnection connection = null;

        public CmdSelectCarHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdSelectCar command, string connectionIdOrNull)
        {
            bool success = true;
            CmdReturnSelectCar response = new CmdReturnSelectCar(command.Id, success);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
