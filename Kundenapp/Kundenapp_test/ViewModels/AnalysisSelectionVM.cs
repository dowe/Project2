using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;

namespace Kundenapp
{
	public class AnalysisSelectionVM : ViewModelBase
	{
		public AnalysisSelectionVM (IClientConnection con)
		{
			connection = con;
			Messenger.Default.Register<PatientM>(this, "edit", p =>{
				Patient = p;
				if(Patient.ListofAnalysis == null)
				{
					Patient.ListofAnalysis = AnalysisList;
				}
				RaisePropertyChanged();
			});
		}

		private IClientConnection connection;

		public PatientM Patient 
		{
			get;
			private set;
		}
			
		private ReadOnlyCollection<Analysis> analysisList;

		public ObservableCollection<AnalysisM> AnalysisList
		{
			get
			{
				if (analysisList == null) 
				{
					CmdReturnGetAnalyses ret = connection.SendWait<CmdReturnGetAnalyses> (new CmdGetAnalyses ());
					analysisList = ret.Analyses;
				}
				ObservableCollection<AnalysisM> anList = new ObservableCollection<AnalysisM> ();
				foreach (Analysis an in analysisList) 
				{
					anList.Add (new AnalysisM (an));
				}
				return anList;
			}
		}
	}
}

