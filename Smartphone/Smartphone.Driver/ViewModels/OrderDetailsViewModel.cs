using System;
using GalaSoft.MvvmLight;
using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using Common.Commands;
using Smartphone.Driver.Messages;
using Smartphone.Driver.Models;

namespace Smartphone.Driver.ViewModels
{
	public class OrderDetailsViewModel : ViewModelBase
	{

		private const string OrderIDProperty = "OrderID";
		private const string CustomerAddressProperty = "CustomerAddress";

		private IClientConnection connection = null;
		private Session session = null;

		private Order order = null;

		private string orderID = null;
		private string customerAddress = null;
		private RelayCommand launchMapCommand = null;
		private RelayCommand collectedCommand = null;

		public OrderDetailsViewModel (IClientConnection connection, Session session)
		{
			this.connection = connection;
			this.session = session;

			Messenger.Default.Register<MsgSetOrderDetailsModel> (this, SetOrder);
		}

		public Order Order
		{
			get {
				return order;
			}
			set {
				if (value != order)
				{
					order = value;
					// Set view model props.
					if (value != null)
					{
						OrderID = value.OrderID.ToString();
					}
					else
					{
						orderID = string.Empty;
					}
					if (value != null && value.Customer != null && value.Customer.Address != null)
					{
						CustomerAddress = value.Customer.Address.Street + "\n" + value.Customer.Address.PostalCode + value.Customer.Address.City;
					}
					else
					{
						CustomerAddress = string.Empty;
					}
				}
			}
		}

		public string OrderID
		{
			get {
				return orderID;
			}
			set {
				if (value != orderID)
				{
					orderID = value;
					RaisePropertyChanged (OrderIDProperty);
				}
			}
		}

		public string CustomerAddress
		{
			get {
				return customerAddress;
			}
			set {
				if (!value.Equals (customerAddress))
				{
					customerAddress = value;
					RaisePropertyChanged (CustomerAddressProperty);
				}
			}
		}

		public RelayCommand LaunchMapCommand
		{
			get {
				if (launchMapCommand == null)
				{
					launchMapCommand = new RelayCommand (LaunchMap);
				}
				return launchMapCommand;
			}
		}

		private void LaunchMap()
		{
			if (order.Customer.Address != null)
			{
				try
				{
					new NativeMapAppLauncher ().LaunchMapApp (order.Customer.Address);
				}
				catch (Exception)
				{
					// TODO show toast or something that no map app could be launched.
				}
			}
		}

		public RelayCommand CollectedCommand
		{
			get {
				if (collectedCommand == null)
				{
					collectedCommand = new RelayCommand (SetCollected);
				}
				return collectedCommand;
			}
		}

		private void SetCollected()
		{
			CmdSetOrderCollected setOrderCollected = new CmdSetOrderCollected (session.Username, order.OrderID);
			CmdReturnSetOrderCollected response = connection.SendWait<CmdReturnSetOrderCollected>(setOrderCollected);
			if (response != null && response.Success)
			{
				// Switch back to order list.
				Messenger.Default.Send<MsgSwitchOrdersPage> (new MsgSwitchOrdersPage ());
			}
			else 
			{
				// TODO show toast or something.
			}
		}

		private void SetOrder(MsgSetOrderDetailsModel msg)
		{
			Order = msg.Order;
		}

	}
}

