using System;
namespace Communication.Client
{
    public interface IClientConnection
    {
        void Send(CommunicationCommon.Command command);
    }
}
