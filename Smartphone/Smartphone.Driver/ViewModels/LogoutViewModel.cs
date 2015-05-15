using System;
using GalaSoft.MvvmLight;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Command;
using Common.Commands;
using GalaSoft.MvvmLight.Messaging;
using Smartphone.Driver.Models;
using Smartphone.Driver.Messages;

namespace Smartphone.Driver.ViewModels
{
	public class LogoutViewModel : ViewModelBase
	{

		private const string EndKmProperty = "EndKm";
		private const string IsCommunicatingProperty = "IsCommunicating";
		private const string IsNotCommunicatingProperty = "IsNotCommunicating";

		private IClientConnection connection = null;
		private Session session = null;

		private float endKm = 3;
		private bool isCommunicating = false;
		private RelayCommand logoutCommand = null;

		public LogoutViewModel (IClientConnection connection, Session session)
		{
			this.connection = connection;
			this.session = session;
		}

		public float EndKm
		{
			get {
				return endKm;
			}
			set {
				if (Math.Abs (value - endKm) > float.Epsilon)
				{
					endKm = value;
					RaisePropertyChanged (EndKmProperty);
				}
			}
		}

		public bool IsCommunicating
		{
			get {
				return isCommunicating;
			}
			set {
				if (value != isCommunicating)
				{
					isCommunicating = value;
					RaisePropertyChanged (IsCommunicatingProperty);
					RaisePropertyChanged (IsNotCommunicatingProperty);
				}
			}
		}

		public bool IsNotCommunicating
		{
			get {
				return !IsCommunicating;
			}
		}

		public RelayCommand LogoutCommand
		{
			get {
				if (logoutCommand == null)
				{
					logoutCommand = new RelayCommand (Logout);
				}
				return logoutCommand;
			}
		}
			
		private void Logout()
		{
			CmdLogoutDriver logoutDriver = new CmdLogoutDriver (session.Username, session.CarID, endKm);
			CmdReturnLogoutDriver response = connection.SendWait<CmdReturnLogoutDriver> (logoutDriver);
			if (response != null)
			{
				if (response.Success)
				{
					OnLogoutSuccessful ();
				}
			}
		}

		private void OnLogoutSuccessful()
		{
			session.Reset ();
			Messenger.Default.Send<MsgSwitchLoginPage> (new MsgSwitchLoginPage ());
		}

	}
}

