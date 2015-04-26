using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Communication
{
    public class ResponseCommandHandler<T> : CommandHandler<T> where T : Command
    {

        private Guid awaitedCommandId = Guid.Empty;
        private AutoResetEvent receivedEvent = null;
        private T receivedCommand = null;

        public ResponseCommandHandler(Guid awaitedCommandId)
        {
            this.awaitedCommandId = awaitedCommandId;
            this.receivedEvent = new AutoResetEvent(false);
        }

        protected override void Handle(T command, string connectionIdOrNull)
        {
            if (awaitedCommandId.Equals(command.Id))
            {
                receivedCommand = command;

                receivedEvent.Set();
            }
        }

        public T WaitForResponse(int waitTimeInMs)
        {
            T result = null;

            if (receivedEvent.WaitOne(waitTimeInMs))
            {
                result = receivedCommand;
            }

            return result;
        }

    }
}
