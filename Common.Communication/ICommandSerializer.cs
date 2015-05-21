using System;
using System.Collections.Generic;
namespace Common.Communication
{
    public interface ICommandSerializer
    {

        string SerializeCommand(Command command);
        Command DeserializeCommandOrNull(string jsonCommand, IEnumerable<Type> parsableCommandTypes);

    }
}
