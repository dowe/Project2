using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Communication.Tests
{
    [TestClass]
    public class ResponseCommandHandlerTests
    {

        private ResponseCommandHandler<DummyCommand> CreateTestee(Guid id)
        {
            return new ResponseCommandHandler<DummyCommand>(id);
        }
        
        [TestMethod]
        public void Test_WaitForResponse_AfterReceivingMatchingResponse_ReturnsResponse()
        {
            var acceptedId = Guid.NewGuid();
            ResponseCommandHandler<DummyCommand> testee = CreateTestee(acceptedId);
            var dummyCommand = new DummyCommand(acceptedId);

            testee.TryHandleCommand(dummyCommand, "Look at me I am a connectionId.");
            var receivedCommand = testee.WaitForResponse(0);

            Assert.AreEqual(dummyCommand, receivedCommand);
        }

        [TestMethod]
        public void Test_WaitForResponse_AfterReceivingResponseWithNotMatchingId_ReturnsNull()
        {
            var acceptedId = Guid.NewGuid();
            ResponseCommandHandler<DummyCommand> testee = CreateTestee(acceptedId);
            var dummyCommand = new DummyCommand(Guid.NewGuid());

            testee.TryHandleCommand(dummyCommand, "Look at me I am a connectionId.");
            var receivedCommand = testee.WaitForResponse(0);

            Assert.IsNull(receivedCommand);
        }

        private class DummyCommand : Command
        {
            public DummyCommand(Guid id) : base(id)
            {
            }
        }

    }
}
