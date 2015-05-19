using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Kundenapp
{
	public class AddDialogVM : ViewModelBase
	{

		public String id {
			get;
			set;
		}

		private RelayCommand addPatientCMD;

		public RelayCommand AddPatientCMD
		{
			get{
				return addPatientCMD ?? (addPatientCMD = new RelayCommand(() =>
					{
						Messenger.Default.Send<String>(id,"add");
						id = null;
						Messenger.Default.Send<NotificationMessage>(new NotificationMessage("AddPop"));
					}));
			}
		}
	}
}

