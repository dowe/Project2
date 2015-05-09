using System;
namespace Common.Communication.Server
{
    public interface IServerConnection
    {
        void Broadcast(Command command);
        void Multicast(Command command, string groupId);
        void Unicast(Command command, string connectionId);
        void InjectInternal(Command command);
    }
}
