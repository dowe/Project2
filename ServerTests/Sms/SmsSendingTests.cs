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
            try
            {
                ISmsSending sms = new SmsSending();
            }
            catch (Exception e)
            {
                Assert.Fail("Gmail Credentials file not found (" + e.Message + ")");
            }
        }
    }
}
