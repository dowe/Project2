using Common.Communication;
using Common.Communication.Client;
using System;

namespace ManagementSoftware.Communication
{
    public class ClientConnectionCreator : ClientConnection
    {
        public ClientConnectionCreator()
            : base("http://localhost:8080/commands")
        {
            Start();
            Connect();
        }
    }

}
