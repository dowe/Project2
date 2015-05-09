﻿using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Common.Commands;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Messaging;

namespace Smartphone.Driver
{
	public class OrdersViewModel : ViewModelBase
	{

		private const string OrdersProperty = "WrappedOrders";

		private IClientConnection connection = null;

		private WrappedOrders wrappedOrders = null;
		private RelayCommand logoutCommand = null;

		public OrdersViewModel(IClientConnection connection, WrappedOrders wrappedOrders)
		{
			this.connection = connection;
			this.wrappedOrders = wrappedOrders;
		}

		public WrappedOrders WrappedOrders
		{
			get {
				return wrappedOrders;
			}
			set {
				wrappedOrders = value;
				RaisePropertyChanged (OrdersProperty);
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
			Messenger.Default.Send<MsgSwitchLogoutPage> (new MsgSwitchLogoutPage ());
		}

	}
}

