using System;
namespace Common.Communication.Server
{
    public interface IServerConnection
    {
        event Action<Command> BeforeHandlingCommand;
        void Broadcast(Command command);
        void Multicast(Command command, string groupId);
        void Unicast(Command command, string connectionId);
        void InjectInternal(Command command);
    }
}
