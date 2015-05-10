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
    class CmdAnnounceEmergencyHandler : CommandHandler<CmdAnnounceEmergency>
    {

        private IServerConnection connection = null;

        public CmdAnnounceEmergencyHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdAnnounceEmergency command, string connectionIdOrNull)
        {
            bool success = true;
            CmdReturnAnnounceEmergency response = new CmdReturnAnnounceEmergency(command.Id, success);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
