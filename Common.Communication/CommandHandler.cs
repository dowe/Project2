using System;

namespace Common.Communication
{
    public abstract class CommandHandler<T> : ICommandHandler where T : Command
    {

        public bool TryHandleCommand(Command command, string connectionIdOrNull)
        {
            bool handled = false;

            if (command == null)
            {
                throw new ArgumentNullException();
            }

            if (command is T)
            {
                T tCommand = (T)command;
                Handle(tCommand, connectionIdOrNull);
                handled = true;
            }

            return handled;
        }

        /// <summary>
        /// Handles the received command that matches the Type T.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connectionIdOrNull">The connection id, this command has been received from or null if it is from the server or injected.</param>
        protected abstract void Handle(T command, string connectionIdOrNull);

        public Type AcceptedType
        {
            get
            {
                return typeof(T);
            }
        }

    }
}
