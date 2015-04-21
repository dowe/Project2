using System;
namespace CommunicationClient
{
    public interface IClientConnection
    {
        void Send(CommunicationCommon.Command command);
    }
}
