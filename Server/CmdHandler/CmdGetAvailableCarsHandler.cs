using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Commands;
using Common.Communication;
using Common.Communication.Server;
using Common.DataTransferObjects;

namespace Server.CmdHandler
{
    class CmdGetAvailableCarsHandler : CommandHandler<CmdGetAvailableCars>
    {

        private IServerConnection connection = null;

        public CmdGetAvailableCarsHandler(IServerConnection connection)
        {
            this.connection = connection;
        }

        protected override void Handle(CmdGetAvailableCars command, string connectionIdOrNull)
        {
            List<Car> availableCars = new List<Car>() { new Car() { CarID = "OG-KP-417", Roadworthy = true } };
            CmdReturnGetAvailableCars response = new CmdReturnGetAvailableCars(command.Id, availableCars);

            connection.Unicast(response, connectionIdOrNull);
        }
    }
}
