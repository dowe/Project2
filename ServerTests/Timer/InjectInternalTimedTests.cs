using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Timer;
using Common.Communication;
using NSubstitute;
using Common.Communication.Server;

namespace ServerTests.Timer
{
    [TestClass]
    public class InjectInternalTimedTests
    {
        [TestMethod]
        public void InjectInternalTimedTest()
        {
            DummyCommand cmd = new DummyCommand();

            var fakeTimer = Substitute.For<ITimer>();
            
            var fakeConnection = Substitute.For<IServerConnection>();

            InjectInternalTimed testee = new InjectInternalTimed(fakeConnection, fakeTimer,() => cmd);
            testee.Start();

            fakeTimer.Received().Start(Arg.Is<Action>(a=>RunAction(a)));

            fakeConnection.Received().InjectInternal(Arg.Is<DummyCommand>(c=>TestCmd(c,cmd)));

            testee.Cancel();

            fakeTimer.Received().Cancel();
        }

        private bool TestCmd(DummyCommand c1, DummyCommand c2)
        {
            return c1 == c2;
        }

        public bool RunAction(Action a)
        {
            a();
            return true;
        }
    }

    class DummyCommand : Command { }
}
