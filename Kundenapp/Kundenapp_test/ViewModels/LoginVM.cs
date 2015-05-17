using System;
using GalaSoft.MvvmLight.Command;
using Common.Communication.Client;
using Common.Commands;
using GalaSoft.MvvmLight.Messaging;

namespace Kundenapp
{
	public class LoginVM
	{
		public LoginVM (IClientConnection connection)
		{
			con = connection;
		}

		IClientConnection con;

		public String Username 
		{
			get;
			set;
		}

		public String Password 
		{
			get;
			set;
		}

		private RelayCommand loginCMD;

		public RelayCommand LoginCMD
		{
			get 
			{
				return loginCMD ?? (loginCMD = new RelayCommand(() =>
					{
						CmdReturnLoginCustomer ret = con.SendWait<CmdReturnLoginCustomer>(new CmdLoginCustomer(Username, Password));
						if(ret != null&&ret.Success)
						{
							Messenger.Default.Send<String>(Username, "Login");
						}
						else
						{
							Messenger.Default.Send<NotificationMessage>(new NotificationMessage("LoginFail"));
						}

					}));
			}
		}
	}
}

