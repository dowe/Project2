using System.Threading.Tasks.Dataflow;

namespace Common.Communication
{
    public class OutputCommandPipeline
    {

        private ICommandSerializer serializer = null;

        private ActionBlock<Sendable<string>> sendBlock = null;

        public OutputCommandPipeline(ICommandSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Start()
        {
            sendBlock = new ActionBlock<Sendable<string>>(sendable =>
                {
                    sendable.Send(sendable.Value);
                });
        }

        public void Send(Sendable<Command> command)
        {
            // Serialization takes place right here because the referenced command might change before being serialized in the pipeline.
            Sendable<string> serializedCommand = Serialize(command);
            sendBlock.Post(serializedCommand);
        }

        private Sendable<string> Serialize(Sendable<Command> command)
        {
            return new Sendable<string>(serializer.SerializeCommand(command.Value), command.Send);
        }

        public void Stop()
        {
            sendBlock.Complete();
            sendBlock.Completion.Wait();
        }

    }
}
