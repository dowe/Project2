using System;
using GalaSoft.MvvmLight;
using Common.Communication.Client;
using Common.DataTransferObjects;
using GalaSoft.MvvmLight.Messaging;

namespace Smartphone.Driver
{
	public class OrderDetailsViewModel : ViewModelBase
	{

		private const string OrderProperty = "Order";

		private IClientConnection connection = null;

		private Order order = null;

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
					RaisePropertyChanged (OrderProperty);
				}
			}
		}

		private void SetOrder(MsgSetOrderDetailsModel msg)
		{
			Order = msg.Order;
		}

	}
}

