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

        private class DummyCommand : Command
        {
        }

        private class TesteeCommandHandler : CommandHandler<DummyCommand>
        {

            public bool HandledCommand { get; private set; }

            protected override void Handle(DummyCommand command, string connectionIdOrNull)
            {
                HandledCommand = true;
            }

        }

    }
}
