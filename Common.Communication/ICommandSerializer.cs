using System;
using System.Collections.Generic;
namespace Common.Communication
{
    public interface ICommandSerializer
    {
        string ParseSerializedCommandType(string jsonCommand);
        string SerializeCommand(Command command);
        Command DeserializeCommand(string jsonCommand, IEnumerable<Type> parsableCommandTypes);
    }
}
