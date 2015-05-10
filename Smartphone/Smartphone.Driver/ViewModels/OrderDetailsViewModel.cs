using System;
using GalaSoft.MvvmLight;
using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight.Messaging;

namespace Smartphone.Driver
{
	public class OrderDetailsViewModel : ViewModelBase
	{

		private const string OrderIDProperty = "OrderID";
		private const string CustomerAddressProperty = "CustomerAddress";

		private IClientConnection connection = null;

		private Order order = null;

		private string orderID = null;
		private string customerAddress = null;

		public OrderDetailsViewModel (IClientConnection connection)
		{
			this.connection = connection;

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
					if (value != null && value.Customer != null)
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

		private void SetOrder(MsgSetOrderDetailsModel msg)
		{
			Order = msg.Order;
		}

	}
}

