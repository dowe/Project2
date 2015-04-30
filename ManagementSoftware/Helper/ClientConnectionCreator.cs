using Common.Communication;
using Common.Communication.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementSoftware.Communication
{
    public class ClientConnectionCreator : ClientConnection
    {
        public ClientConnectionCreator()
            : base("http://localhost:8080/echo")
        {
            Start();
            Connect();
        }
    }

    public class ClientConnectionCreatorDummy : IClientConnection
    {
        public void Send(Command command)
        {

        }
    }
}
