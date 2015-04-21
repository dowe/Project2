using System;
namespace CommunicationServer
{
    public interface IServerConnection
    {
        void Broadcast(CommunicationCommon.Command command);
        void Multicast(CommunicationCommon.Command command, string groupId);
        void Unicast(CommunicationCommon.Command command, string connectionId);
    }
}
