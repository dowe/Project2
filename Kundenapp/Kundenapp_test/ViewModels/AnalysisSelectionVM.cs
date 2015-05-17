using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace Kundenapp
{
	public class AnalysisSelectionVM : ViewModelBase
	{
		public AnalysisSelectionVM ()
		{
			Messenger.Default.Register<PatientM>(this, "edit", p =>{
				Patient = p;
				RaisePropertyChanged();
			});
		}

		public PatientM Patient 
		{
			get;
			private set;
		}
	}
}

