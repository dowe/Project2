using System;
namespace Common.Communication.Client
{
    public interface IClientConnection
    {
        void Send(Command command);
    }
}
