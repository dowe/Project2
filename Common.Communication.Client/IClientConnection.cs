using System;
using Microsoft.AspNet.SignalR.Client;

namespace Common.Communication.Client
{
    public interface IClientConnection
    {
        event Action Closed;
        event Action Reconnecting;
        event Action Reconnected;
        void Start();
        void Stop();
        void Connect();
        void Connect(string serverAddress);
        void Send(Command command);
        T SendWait<T>(Command command) where T : Command;
        T SendWait<T>(Command command, int timeout) where T : Command;
        void RegisterCommandHandler(ICommandHandler handler);
        ConnectionState ConnectionState{ get; }
        string ServerURL { get; }
    }
}