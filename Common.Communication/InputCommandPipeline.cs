using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Common.Communication
{
    public class InputCommandPipeline : IInputCommandPipeline
    {

        private ICommandSerializer serializer = null;
        private CommandHandlerList handlers = null;
        private CommandHandlerList responseHandlers = null;
        private ConcurrentDictionary<Type, byte> parsableTypes = null;

        private TransformBlock<Received<string>, Received<Command>> deserializeBlock = null;
        private TransformBlock<Command, Received<Command>> injectBlock = null;
        private BroadcastBlock<Received<Command>> broadcastHandleBlock = null;
        private ActionBlock<Received<Command>> handleBlock = null;
        private ActionBlock<Received<Command>> waitHandleBlock = null;

        public InputCommandPipeline(ICommandSerializer serializer) : this(serializer, new CommandHandlerList(), new CommandHandlerList(), new ConcurrentDictionary<Type, byte>())
        {
        }

        InputCommandPipeline(ICommandSerializer serializer, CommandHandlerList handlers, CommandHandlerList waitHandlers, ConcurrentDictionary<Type, byte> parsableTypes)
        {
            this.serializer = serializer;
            this.handlers = handlers;
            this.responseHandlers = waitHandlers;
            this.parsableTypes = parsableTypes;
        }

        public void Start()
        {
            deserializeBlock = new TransformBlock<Received<string>, Received<Command>>(receivedMessage =>
                {
                    return new Received<Command>(serializer.DeserializeCommand(receivedMessage.Value, parsableTypes.Keys), receivedMessage.ConnectionIdOrNull);
                });
            injectBlock = new TransformBlock<Command, Received<Command>>(command =>
                {
                    return new Received<Command>(command, null);
                });
            broadcastHandleBlock = new BroadcastBlock<Received<Command>>(command =>
                {
                    return new Received<Command>(command);
                });
            handleBlock = new ActionBlock<Received<Command>>(receivedCommand =>
                {
                    handlers.TryHandleCommand(receivedCommand.Value, receivedCommand.ConnectionIdOrNull);
                });
            waitHandleBlock = new ActionBlock<Received<Command>>(receivedCommand =>
                {
                    responseHandlers.TryHandleCommand(receivedCommand.Value, receivedCommand.ConnectionIdOrNull);
                });

            deserializeBlock.LinkTo(broadcastHandleBlock);
            injectBlock.LinkTo(broadcastHandleBlock);
            broadcastHandleBlock.LinkTo(handleBlock);
            broadcastHandleBlock.LinkTo(waitHandleBlock);
            // Complete the handle block when both, deserialization and injection is completed.
            Task.WhenAll(deserializeBlock.Completion, injectBlock.Completion).ContinueWith(_ => broadcastHandleBlock.Complete());
            broadcastHandleBlock.Completion.ContinueWith(_ =>
                {
                    handleBlock.Complete();
                    waitHandleBlock.Complete();
                });
        }

        /// <summary>
        /// Add a received, serialized command to the pipeline.
        /// </summary>
        /// <param name="receivedMessage"></param>
        public void AddReceived(string message, string connectionIdOrNull)
        {
            Received<string> receivedMessage = new Received<string>(message, connectionIdOrNull);
            deserializeBlock.Post(receivedMessage);
        }

        /// <summary>
        /// Inject a command into the pipeline without having received it from any connection.
        /// </summary>
        /// <param name="command"></param>
        public void InjectInternal(Command command)
        {
            injectBlock.Post(command);
        }

        public void Stop()
        {
            deserializeBlock.Complete();
            injectBlock.Complete();
            // At this point, no more items can be added to the pipeline's input.
            // Handle block complete will propagate.
            // Wait until all left items in the pipeline are handled.
            handleBlock.Completion.Wait();
            waitHandleBlock.Completion.Wait();
        }

        public void RegisterCommandHandler(ICommandHandler handler)
        {
            handlers.RegisterCommandHandler(handler);
            parsableTypes.AddOrUpdate(handler.AcceptedType, 0, (k, v) => 0);
        }

        public void DeregisterCommandHandler(ICommandHandler handler)
        {
            handlers.DeregisterCommandHandler(handler);
        }

        public void RegisterResponseCommandHandler<T>(ResponseCommandHandler<T> responseHandler) where T : Command
        {
            responseHandlers.RegisterCommandHandler(responseHandler);
            parsableTypes.AddOrUpdate(responseHandler.AcceptedType, 0, (k, v) => 0);
        }

        public void DeregisterResponseCommandHandler<T>(ResponseCommandHandler<T> responseHandler) where T : Command
        {
            responseHandlers.DeregisterCommandHandler(responseHandler);
        }

    }

    class Received<T>
    {

        private string connectionIdOrNull;
        private T t;

        public Received(T t, string connectionIdOrNull)
        {
            this.t = t;
            this.connectionIdOrNull = connectionIdOrNull;
        }

        public Received(Received<T> receivedToClone)
        {
            this.t = receivedToClone.t;
            this.connectionIdOrNull = receivedToClone.connectionIdOrNull;
        }

        public string ConnectionIdOrNull
        {
            get
            {
                return connectionIdOrNull;
            }
        }

        public T Value
        {
            get
            {
                return t;
            }
        }
    }
}
