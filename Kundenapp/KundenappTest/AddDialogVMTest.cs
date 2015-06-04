using NUnit.Framework;
using System;
using Kundenapp;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace KundenappTest
{
	[TestFixture ()]
	public class AddDialogVMTest
	{
		private String id;

		[Test ()]
		public void AddCMDTest ()
		{
			Messenger.Default.Register<String> (this, "add", addPatient);
			AddDialogVM addvm = new AddDialogVM ();
			addvm.id = "Test";
			addvm.AddPatientCMD.Execute(null);
			Assert.AreEqual ("Test", id);
		}

		public void addPatient(String id)
		{
			this.id = id;
		}
	}
}

