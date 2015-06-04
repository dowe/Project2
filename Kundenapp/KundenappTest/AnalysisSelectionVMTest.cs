using NUnit.Framework;
using System;
using Kundenapp;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace KundenappTest
{
	[TestFixture ()]
	public class AnalysisSelectionVMTest
	{
		[Test ()]
		public void editPatTest ()
		{
			PatientM pat = new PatientM ("asdf");
			AnalysisSelectionVM avm = new AnalysisSelectionVM (new ClientConnectionMock ());
			Messenger.Default.Send<PatientM> (pat, "edit");
			Assert.IsNotNull (pat.ListofAnalysis);
			Assert.AreEqual (pat.ListofAnalysis.Count, 16);
		}
	}
}

