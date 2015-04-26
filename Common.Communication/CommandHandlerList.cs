using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Common.Communication
{
    class CommandHandlerList
    {

        private ConcurrentDictionary<ICommandHandler, byte> handlers = null;

        public CommandHandlerList()
        {
            handlers = new ConcurrentDictionary<ICommandHandler, byte>();
        }

        public void RegisterCommandHandler(ICommandHandler commandHandler)
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException();
            }
            handlers.AddOrUpdate(commandHandler, 0, (k, v) => 0);
        }

        public void DeregisterCommandHandler(ICommandHandler commandHandler)
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException();
            }
            byte dummyOut;
            handlers.TryRemove(commandHandler, out dummyOut);
            // Do not remove it from the parsableTypes Dictionary. Only Add.
            // Every Type is only added once and by removing it, the command could not be parsed even
            // if there where still handlers that accepted it.
        }

        public bool TryHandleCommand(Command command, string connectionIdOrNull)
        {
            bool handled = false;

            foreach (ICommandHandler handler in handlers.Keys)
            {
                if (handler.AcceptedType.Equals(command.GetType()))
                {
                    handler.TryHandleCommand(command, connectionIdOrNull);
                    handled = true;
                }
            }

            return handled;
        }

    }
}
