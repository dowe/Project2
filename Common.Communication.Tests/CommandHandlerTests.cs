using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Communication.Tests
{
    [TestClass]
    public class CommandHandlerTests
    {

        private TesteeCommandHandler CreateTesteeCommandHandler()
        {
            return new TesteeCommandHandler();
        }

        [TestMethod]
        public void Test_AcceptedType_OnCall_ReturnsGenericType()
        {
            TesteeCommandHandler testee = CreateTesteeCommandHandler();

            Assert.AreEqual(typeof (DummyCommand), testee.AcceptedType);
        }

        [TestMethod]
        public void Test_TryHandleCommand_WithAcceptedCommandType_HandlesCommand()
        {
            TesteeCommandHandler testee = CreateTesteeCommandHandler();
            var dummyCommand = new DummyCommand();
            var dummyConnectionString = "Look at me I am a connectionId.";
            testee.TryHandleCommand(dummyCommand, dummyConnectionString);

            Assert.IsTrue(testee.DidHandleCommand);
            Assert.AreEqual(dummyCommand, testee.HandledCommand);
            Assert.AreEqual(dummyConnectionString, testee.HandledConnectionIdOrNull);
        }


        private class DummyCommand : Command
        {
        }

        private class TesteeCommandHandler : CommandHandler<DummyCommand>
        {

            public bool DidHandleCommand { get; private set; }
            public DummyCommand HandledCommand { get; private set; }
            public string HandledConnectionIdOrNull { get; private set; }

            protected override void Handle(DummyCommand command, string connectionIdOrNull)
            {
                DidHandleCommand = true;
                HandledCommand = command;
                HandledConnectionIdOrNull = connectionIdOrNull;
            }

        }

    }
}
