using System;
namespace Common.Communication
{
    public interface IInputCommandPipeline
    {
        void AddReceived(string message, string connectionIdOrNull);
        void InjectInternal(Command command);
    }
}
