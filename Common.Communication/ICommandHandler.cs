using System;
namespace Common.Communication
{
    public interface ICommandHandler
    {

        Type AcceptedType
        {
            get;
        }

        /// <summary>
        /// Tries to handle the command if it matches the accepted type.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connectionIdOrNull"></param>
        /// <returns>Whether the command was successfully handled.</returns>
        bool TryHandleCommand(Command command, string connectionIdOrNull);

    }
}
