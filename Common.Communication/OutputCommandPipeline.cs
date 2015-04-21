using System.Threading.Tasks.Dataflow;

namespace Common.Communication
{
    public class OutputCommandPipeline
    {

        private ICommandSerializer serializer = null;

        private TransformBlock<Sendable<Command>, Sendable<string>> serializeBlock = null;
        private ActionBlock<Sendable<string>> sendBlock = null;

        public OutputCommandPipeline(ICommandSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Start()
        {
            serializeBlock = new TransformBlock<Sendable<Command>, Sendable<string>>(cmd =>
                {
                    return new Sendable<string>(serializer.SerializeCommand(cmd.Value), cmd.Send);
                });
            sendBlock = new ActionBlock<Sendable<string>>(sendable => 
                {
                    sendable.Send(sendable.Value);
                });
            serializeBlock.Completion.ContinueWith(t => 
                {
                    sendBlock.Complete();
                });

            serializeBlock.LinkTo(sendBlock);
        }

        public void Send(Sendable<Command> command)
        {
            serializeBlock.Post(command);
        }

        public void Stop()
        {
            serializeBlock.Complete();
            sendBlock.Completion.Wait();
        }

    }
}
