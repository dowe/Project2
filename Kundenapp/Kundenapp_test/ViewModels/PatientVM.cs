using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;


namespace Kundenapp
{
	public class PatientVM : ViewModelBase
	{

		public ObservableCollection<PatientM> Patients {
			get;
			set;
		}

		public PatientVM(IClientConnection connection)
		{
			this.connection = connection;
			Patients = new ObservableCollection<PatientM> ();
			Messenger.Default.Register<String> (this, "add", addPatient);
			Messenger.Default.Register<PatientM> (this, "delPat", delPatient);
			Messenger.Default.Register<String> (this, "Username", (str) => username = str);
		}

		private String username;

		private IClientConnection connection;

		private RelayCommand addCMD;

		public RelayCommand AddCMD
		{
			get{
				return addCMD ?? (addCMD = new RelayCommand (
					() => Messenger.Default.Send (new NotificationMessage ("Add"))));
				
			}
		}

		private RelayCommand sendCMD;

		public RelayCommand SendCMD
		{
			get{
				return sendCMD ?? (sendCMD = new RelayCommand (
					() => 
					{
						Dictionary<String, List<Analysis>> dic = new Dictionary<String, List<Analysis>>();
						foreach(PatientM Pat in Patients)
						{
							List<Analysis> list = new List<Analysis>();
							foreach(AnalysisM an in Pat.ListofAnalysis)
							{
								if(an.Selected)
								{
									list.Add(an.Analysis);
								}
							}
							dic.Add(Pat.PatientID,list);
						}
						CmdAddOrder order = new CmdAddOrder(dic,username);
						CmdReturnAddOrder retorder = connection.SendWait<CmdReturnAddOrder>(order);
						Patients.Clear();
						Messenger.Default.Send<long>(retorder.CreatedOrderId);
						//CmdReturnLogoutCustomer retlogout = connection.SendWait(new CmdLogoutCustomer(username));
					}
				));

			}
		}

		public void addPatient(String id)
		{
			bool exists = false;
			foreach (PatientM Pat in Patients)
				if (Pat.PatientID == id)
					exists = true;

			if (String.IsNullOrWhiteSpace(id)||exists) 
			{
				Messenger.Default.Send (new NotificationMessage ("exists"));
			}
			else
			{
				Patients.Add (new PatientM (id));
			}
		}

		public void delPatient(PatientM Patient)
		{
			Patients.Remove (Patient);
		}
	}
}

