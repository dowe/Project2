using Server.Sms;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTests.Sms
{
    [TestClass]
    public class SmsSendingTests
    {
        [TestMethod]
        public void CheckIfGmailAccountCredentialsFileExists()
        {
            ISmsSending sms = new SmsSending();
            Assert.IsTrue(sms.Enabled);
        }
    }
}
