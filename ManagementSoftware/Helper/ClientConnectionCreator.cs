using Common.Communication;
using Common.Communication.Client;
using System;

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

        public event Action Closed;
        public event Action Reconnecting;
        public event Action Reconnected;

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Connect()
        {
        }

        public void Send(Command command)
        {
        }

        public T SendWait<T>(Command command) where T : Command
        {
            return null;
        }

        public T SendWait<T>(Command command, int timeout) where T : Command
        {
            return null;
        }

        public void RegisterCommandHandler(ICommandHandler handler)
        {
        }

    }
}
