using System;
using System.Collections.Generic;
using System.Linq;
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
            fakeHandler.AcceptedType.Returns(typeof (DummyCommand));
            testee.RegisterCommandHandler(fakeHandler);
            testee.Start();

            testee.InjectInternal(dummyCommand);

            testee.Stop();
            fakeHandler.Received().TryHandleCommand(dummyCommand, null);
        }

        [TestMethod]
        public void InjectInternal_WithRegisteredNotMatchingCommandHandlers_CallsCommandHandlers()
        {
            var testee = CreateTestee(CreateDummmySerializer());
            var dummyCommand = new DummyCommand();
            var fakeHandler = Substitute.For<ICommandHandler>();
            fakeHandler.AcceptedType.Returns(typeof (OtherDummyCommand));
            testee.RegisterCommandHandler(fakeHandler);
            testee.Start();

            testee.InjectInternal(dummyCommand);

            testee.Stop();
            fakeHandler.DidNotReceive().TryHandleCommand(dummyCommand, null);
        }

        [TestMethod]
        public void InjectInternal_WithDeregisteredMatchingCommandHandlers_DoesNotCallCommandHandlers()
        {
            var testee = CreateTestee(CreateDummmySerializer());
            var dummyCommand = new DummyCommand();
            var fakeHandler = Substitute.For<ICommandHandler>();
            fakeHandler.AcceptedType.Returns(typeof (DummyCommand));
            var fakeResponseHandler = Substitute.For<ICommandHandler>();
            fakeResponseHandler.AcceptedType.Returns(typeof (DummyCommand));
            testee.RegisterCommandHandler(fakeHandler);
            testee.RegisterResponseCommandHandler(fakeResponseHandler);
            testee.Start();
            testee.DeregisterCommandHandler(fakeHandler);
            testee.DeregisterResponseCommandHandler(fakeResponseHandler);

            testee.InjectInternal(dummyCommand);

            testee.Stop();
            fakeHandler.DidNotReceive().TryHandleCommand(dummyCommand, null);
            fakeResponseHandler.DidNotReceive().TryHandleCommand(dummyCommand, null);
        }

        [TestMethod]
        public void AddReceived_WithMatchingCommandHandler_CallsCommandHandlerCommandHandler()
        {
            var connectionSource = "Look at me I am the sender.";
            var serializedCommand = "Look at me I am Json.";
            var deserializedCommand = new DummyCommand();
            var fakeSerializer = Substitute.For<ICommandSerializer>();
            fakeSerializer.DeserializeCommand(serializedCommand, Arg.Any <IEnumerable<Type>>()).Returns(deserializedCommand);
            var fakeHandler = Substitute.For<ICommandHandler>();
            fakeHandler.AcceptedType.Returns(typeof (DummyCommand));
            var fakeResponseHandler = Substitute.For<ICommandHandler>();
            fakeResponseHandler.AcceptedType.Returns(typeof (DummyCommand));
            var testee = CreateTestee(fakeSerializer);
            testee.RegisterCommandHandler(fakeHandler);
            testee.Start();
            testee.RegisterResponseCommandHandler(fakeResponseHandler);

            testee.AddReceived(serializedCommand, connectionSource);

            testee.Stop();
            fakeHandler.Received().TryHandleCommand(deserializedCommand, connectionSource);
            fakeResponseHandler.Received().TryHandleCommand(deserializedCommand, connectionSource);
        }

        private class DummyCommand : Command
        {
        }

        private class OtherDummyCommand : Command
        {
        }

    }
}
