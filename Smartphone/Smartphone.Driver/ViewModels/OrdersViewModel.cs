using System;
using Common.DataTransferObjects;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace Smartphone.Driver
{
	public class OrdersViewModel : ViewModelBase
	{

		private const string OrdersProperty = "WrappedOrders";

		private WrappedOrders wrappedOrders = null;

		public OrdersViewModel(WrappedOrders wrappedOrders)
		{
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
	}
}

