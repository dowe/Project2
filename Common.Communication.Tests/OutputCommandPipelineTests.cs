using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Common.Communication.Tests
{
    [TestClass]
    public class OutputCommandPipelineTests
    {

        private OutputCommandPipeline CreateTestee(ICommandSerializer serializer)
        {
            return new OutputCommandPipeline(serializer);
        }

        [TestMethod]
        public void Test_SendSendable_CallsSendWithSerializedCommand()
        {
            bool sentSerializedDummy = false;
            var command = new DummyCommand();
            var serializedCommand = "Look at me I am serialized.";
            Sendable<Command> sendableDummy = new Sendable<Command>(command, s =>
            {
                if (s.Equals(serializedCommand)) sentSerializedDummy = true;
            });
            var fakeSerializer = Substitute.For<ICommandSerializer>();
            fakeSerializer.SerializeCommand(command).Returns(serializedCommand);
            var testee = CreateTestee(fakeSerializer);
            testee.Start();

            testee.Send(sendableDummy);

            testee.Stop();

            Assert.IsTrue(sentSerializedDummy);
        }

        private class DummyCommand : Command
        {
        }
    }
}
