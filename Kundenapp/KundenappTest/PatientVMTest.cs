using NUnit.Framework;
using System;
using Kundenapp;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;

namespace KundenappTest
{
	[TestFixture ()]
	public class PatientVMTest
	{

		[Test ()]
		public void  addPatientTest()
		{
			PatientVM pvm = new PatientVM (new ClientConnectionMock ());
			Messenger.Default.Send<String> ("asdf", "add");
			Assert.AreEqual (pvm.Patients.Count, 1);
		}

		[Test ()]
		public void  delPatientTest()
		{
			PatientVM pvm = new PatientVM (new ClientConnectionMock ());
			PatientM p = new PatientM ("asdf");
			pvm.Patients.Add(p);
			Messenger.Default.Send<PatientM> (p, "delPat");
			Assert.AreEqual (pvm.Patients.Count, 0);
		}

		[Test ()]
		public void  AddCMDTest()
		{
			PatientVM pvm = new PatientVM (new ClientConnectionMock ());
			bool success = false;
			Messenger.Default.Register<NotificationMessage>(this, (not) => {
				if(not.Notification == "Add")
					success = true;
			});
			pvm.AddCMD.Execute (null);
			Assert.AreEqual (true, success);
		}

		[Test ()]
		public void  SendCMDTest()
		{
			long id = 0;
			Messenger.Default.Register<long> (this, (l) => id = l);
			ClientConnectionMock mock = new ClientConnectionMock ();
			PatientVM pvm = new PatientVM (mock);
			Messenger.Default.Send ("abc", "Username");
			PatientM pat1 = new PatientM("asdf");
			pat1.ListofAnalysis = new ObservableCollection<AnalysisM>(){ new AnalysisM (new Analysis (){ Name = "An1" }){ Selected = true }};
			PatientM pat2 = new PatientM("asdf2");
			pat2.ListofAnalysis = new ObservableCollection<AnalysisM>(){new AnalysisM (new Analysis (){ Name = "An2" }){ Selected = true }};
			pvm.Patients.Add (pat1);
			pvm.Patients.Add (pat2);
			pvm.SendCMD.Execute (null);
			Assert.AreEqual (1, id);
			Assert.AreEqual (mock.cmd.CustomerUsername, "abc");
			Assert.AreEqual (mock.cmd.PatientTests.Count, 2);
			Assert.AreEqual (mock.cmd.PatientTests ["asdf"] [0].Name, "An1");
			Assert.AreEqual (mock.cmd.PatientTests["asdf2"][0].Name, "An2");
		}
	}
}

