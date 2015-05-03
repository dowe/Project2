using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Common.Communication.Client;

namespace Smartphone.Driver
{
	public class LoginViewModel : ViewModelBase
	{

		public const string UsernameProperty = "Username";
		public const string PasswordProperty = "Password";
		public const string IsConnectedProperty = "IsConnected";
		public const string IsCommunicatingProperty = "IsCommunicating";

		private IClientConnection connection = null;

		private string username = null;
		private string password = null;
		private bool connected = false;
		private bool communicating = false;
		private RelayCommand login = null;

		public LoginViewModel (IClientConnection connection)
		{
			this.connection = connection;

			username = string.Empty;
			password = string.Empty;
			communicating = false;
		}

		public string Username
		{
			get { return username; }
			set {
				if (!string.Equals (username, value))
				{
					username = value;
					RaisePropertyChanged (UsernameProperty);
				}
			}
		}

		public string Password
		{
			get { return password; }
			set {
				if (!string.Equals (password, value))
				{
					password = value;
					RaisePropertyChanged (PasswordProperty);
				}
			}
		}

		public bool IsConnected
		{
			get { return connected; }
			set {
				if (connected != value)
				{
					connected = value;
					RaisePropertyChanged (IsConnectedProperty);
				}
			}
		}

		public bool IsCommunicating
		{
			get { return communicating; }
			set {
				if (communicating != value)
				{
					communicating = value;
					RaisePropertyChanged (IsCommunicatingProperty);
				}
			}
		}

		public RelayCommand Login
		{
			get {
				return login ?? (login = new RelayCommand (() =>
				{
						// TODO: Send login command and switch page if successfully.
				}));
			}
		}
	}
}

