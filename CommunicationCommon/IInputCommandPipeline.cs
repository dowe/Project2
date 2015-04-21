using System;
namespace CommunicationCommon
{
    public interface IInputCommandPipeline
    {
        void AddReceived(string message, string connectionIdOrNull);
        void InjectInternal(Command command);
    }
}
