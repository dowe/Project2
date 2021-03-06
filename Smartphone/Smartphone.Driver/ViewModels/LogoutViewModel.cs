﻿using System;
using GalaSoft.MvvmLight;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Command;
using Common.Commands;
using GalaSoft.MvvmLight.Messaging;
using Smartphone.Driver.Const;
using Smartphone.Driver.Models;
using Smartphone.Driver.Messages;
using Smartphone.Driver.GPS;
using Smartphone.Driver.NativeServices;

namespace Smartphone.Driver.ViewModels
{
	public class LogoutViewModel : ViewModelBase
	{

		private const string EndKmProperty = "EndKm";
		private const string IsCommunicatingProperty = "IsCommunicating";
		private const string IsNotCommunicatingProperty = "IsNotCommunicating";

		private IClientConnection connection = null;
		private Session session = null;
		private GPSPositionSender gpsSender = null;
	    private IToaster toaster = null;

		private float endKm = 3;
		private bool isCommunicating = false;
		private RelayCommand logoutCommand = null;

		public LogoutViewModel (IClientConnection connection, Session session, GPSPositionSender gpsSender, IToaster toaster)
		{
			this.connection = connection;
			this.session = session;
			this.gpsSender = gpsSender;
		    this.toaster = toaster;
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
		            OnLogoutSuccessful();
		        }
		        else
		        {
		            if (endKm < response.MinKm)
		            {
		                toaster.MakeToast(TextDefinitions.KmIsLowerThanDB(response.MinKm));
		            }
		            else
		            {
                        toaster.MakeToast(TextDefinitions.FAILED_LOGOUT);
		            }
		        }
		    }
		    else
		    {
		        toaster.MakeToast(TextDefinitions.SERVER_NO_ANSWER_RELOGIN);
                // Prevent inconsistent state.
                session.Reset();
		        Messenger.Default.Send<MsgSwitchLoginPage>(new MsgSwitchLoginPage());
		    }
		}

		private void OnLogoutSuccessful()
		{
			gpsSender.Stop ();

			session.Reset ();

			Messenger.Default.Send<MsgSwitchLoginPage> (new MsgSwitchLoginPage ());
		}

	}
}

