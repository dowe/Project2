using System;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Common.Communication.Client;
using Common.DataTransferObjects;
using System.Collections.Generic;

namespace Smartphone.Driver
{
	public class ViewModelLocator
	{
		
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			WrappedOrders orders = new WrappedOrders ();
			// TODO Remove this dummy data.
			Order order = new Order();
			orders.UpdateAll (new List<Order> () { order });
			SimpleIoc.Default.Register<WrappedOrders> (() => orders);

			IClientConnection clientConnection = new ClientConnection ("http://192.168.56.1:8080/commands");
			clientConnection.RegisterCommandHandler (new CmdReturnGetDriversUnfinishedOrdersHandler (orders));
			clientConnection.Start ();
			SimpleIoc.Default.Register<IClientConnection> (() => clientConnection);
			
			SimpleIoc.Default.Register<LoginViewModel> ();
			SimpleIoc.Default.Register<OrdersViewModel> ();
		}
			
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public LoginViewModel Login
		{
			get
			{
				return ServiceLocator.Current.GetInstance<LoginViewModel>();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public OrdersViewModel Orders
		{
			get {
				return ServiceLocator.Current.GetInstance<OrdersViewModel> ();
			}
		}

	}
}