using System;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Common.Communication.Tests
{
    [TestClass]
    public class InputCommandPipelineTests
    {

        private InputCommandPipeline CreateTestee(ICommandSerializer serializer)
        {
            return new InputCommandPipeline(serializer);
        }

        private ICommandSerializer CreateDummmySerializer()
        {
            return Substitute.For<ICommandSerializer>();
        }

        [TestMethod]
        public void InjectInternal_WithRegisteredMatchingCommandHandler_CallsCommandHandler()
        {
            var testee = CreateTestee(CreateDummmySerializer());
            var dummyCommand = new DummyCommand();
            var fakeHandler = Substitute.For<ICommandHandler>();
            fakeHandler.TryHandleCommand(dummyCommand, null).Returns(true);
            fakeHandler.AcceptedType.Returns(typeof (DummyCommand));
            testee.RegisterCommandHandler(fakeHandler);
            testee.Start();

            testee.InjectInternal(dummyCommand);

            testee.Stop();
            fakeHandler.Received().TryHandleCommand(dummyCommand, null);
        }

        [TestMethod]
        public void InjectInternal_WithRegisteredNotMatchingCommandHandler_CallsCommandHandler()
        {
            var testee = CreateTestee(CreateDummmySerializer());
            var dummyCommand = new DummyCommand();
            var fakeHandler = Substitute.For<ICommandHandler>();
            fakeHandler.TryHandleCommand(dummyCommand, null).Returns(false);
            fakeHandler.AcceptedType.Returns(typeof (OtherDummyCommand));
            testee.RegisterCommandHandler(fakeHandler);
            testee.Start();

            testee.InjectInternal(dummyCommand);

            testee.Stop();
            fakeHandler.DidNotReceive().TryHandleCommand(dummyCommand, null);
        }

        private class DummyCommand : Command
        {
        }

        private class OtherDummyCommand : Command
        {
        }
    }
}
