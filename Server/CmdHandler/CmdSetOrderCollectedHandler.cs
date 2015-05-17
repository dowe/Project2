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
    class CmdSetOrderCollectedHandler : CommandHandler<CmdSetOrderCollected>
    {

        private IServerConnection connection = null;

        public CmdSetOrderCollectedHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdSetOrderCollected command, string connectionIdOrNull)
        {
            bool success = true;
            CmdReturnSetOrderCollected response = new CmdReturnSetOrderCollected(command.Id, success);
            
            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
