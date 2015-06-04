using NUnit.Framework;
using System;
using Kundenapp;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace KundenappTest
{
	[TestFixture ()]
	public class LoginVMTest
	{
		private string name;
		private bool failed;

		[Test ()]
		public void LoginCMDTest ()
		{
			Messenger.Default.Register<String> (this, "Login", (str) => {
				name = str;
			});
			ClientConnectionMock mock = new ClientConnectionMock ();
			mock.retbool = true;
			LoginVM lvm = new LoginVM (mock);
			lvm.Username = "asdf";
			lvm.Password = "asdf";
			lvm.LoginCMD.Execute (null);
			Assert.AreEqual ("asdf", name);
		}

		[Test ()]
		public void LoginCMDFailedTest ()
		{
			Messenger.Default.Register<NotificationMessage> (this, (msg) => {
				if(msg.Notification == "LoginFail")
				{
					failed = true;
				}
			});
			ClientConnectionMock mock = new ClientConnectionMock ();
			mock.retbool = false;
			LoginVM lvm = new LoginVM (mock);
			lvm.Username = "asdf";
			lvm.Password = "asdf";
			lvm.LoginCMD.Execute (null);
			Assert.AreEqual (true, failed);
		}
	}
}

