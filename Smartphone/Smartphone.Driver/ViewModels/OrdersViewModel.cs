using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Smartphone.Driver
{
	public class OrdersViewModel : ViewModelBase
	{

		private const string OrdersProperty = "Orders";

		private WrappedOrders orders = null;

		public OrdersViewModel()
		{
			orders = new WrappedOrders ();
		}

		public WrappedOrders Orders
		{
			get {
				return orders;
			}
			set {
				orders = value;
				RaisePropertyChanged (OrdersProperty);
			}
		}
	}
}

