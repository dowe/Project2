using System;
using System.Collections.Generic;
namespace CommunicationCommon
{
    public interface ICommandSerializer
    {
        string ParseSerializedCommandType(string jsonCommand);
        string SerializeCommand(Command command);
        Command DeserializeCommand(string jsonCommand, IEnumerable<Type> parsableCommandTypes);
    }
}
