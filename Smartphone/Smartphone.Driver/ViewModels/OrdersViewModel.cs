using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Common.Commands;
using Common.Communication.Client;
using GalaSoft.MvvmLight.Messaging;
using Smartphone.Driver.Models;
using Smartphone.Driver.Messages;
using Smartphone.Driver.GPS;
using Smartphone.Driver.NativeServices;

namespace Smartphone.Driver.ViewModels
{
	public class OrdersViewModel : ViewModelBase
	{

		private const string OrdersProperty = "WrappedOrders";
		private const string SelectedOrderProperty = "SelectedOrder";

		private IClientConnection connection = null;
		private Session session = null;
		private GPSPositionSender gpsSender = null;
		private IToaster toaster = null;

		private WrappedOrders wrappedOrders = null;
		private Order selectedOrder = null;
		private RelayCommand logoutCommand = null;
		private RelayCommand emergencyCommand = null;

		public OrdersViewModel(IClientConnection connection, Session session, WrappedOrders wrappedOrders, GPSPositionSender gpsSender, IToaster toaster)
		{
			this.connection = connection;
			this.session = session;
			this.wrappedOrders = wrappedOrders;
			this.gpsSender = gpsSender;
			this.toaster = toaster;
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

		public Order SelectedIndex
		{
			get {
				return selectedOrder;
			}
			set {
				selectedOrder = value;
				if (value != null)
				{
					// Show order details.
					Messenger.Default.Send (new MsgSwitchOrderDetailsPage (selectedOrder));
				}
				RaisePropertyChanged (SelectedOrderProperty);
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

		public RelayCommand EmergencyCommand
		{
			get {
				if (emergencyCommand == null)
				{
					emergencyCommand = new RelayCommand (Emergency);
				}
				return emergencyCommand;
			}
		}

		private void Logout()
		{
			Messenger.Default.Send<MsgSwitchLogoutPage> (new MsgSwitchLogoutPage ());
		}

		private void Emergency()
		{
			Messenger.Default.Send<MsgShowEmergencyDialog> (new MsgShowEmergencyDialog (OnConfirmedEmergency, OnCanceledEmergency));
		}

		public void OnConfirmedEmergency()
		{
			CmdAnnounceEmergency announceEmergency = new CmdAnnounceEmergency (session.Username, session.CarID);
			CmdReturnAnnounceEmergency response = connection.SendWait<CmdReturnAnnounceEmergency> (announceEmergency);
			if (response != null)
			{
				if (response.Success)
				{
					OnEmergencySuccessful ();
				} else
				{
					toaster.MakeToast ("Emergerncy failed.");
				}
			}
			else
			{
				toaster.MakeToast ("Server unreachable.");
			}

		}

		public void OnCanceledEmergency()
		{
			// Nothing.
		}

		private void OnEmergencySuccessful()
		{
			gpsSender.Stop ();

			session.Reset ();

			Messenger.Default.Send<MsgSwitchLoginPage> (new MsgSwitchLoginPage ());	
		}

	}
}

