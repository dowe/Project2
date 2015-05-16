﻿using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Common.Communication.Client;
using Smartphone.Driver.Models;
using Smartphone.Driver.Handlers;
using Smartphone.Driver.ViewModels;
using Smartphone.Driver.GPS;

namespace Smartphone.Driver.ViewModels
{
	public class ViewModelLocator
	{
		
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			Session session = new Session ();
			SimpleIoc.Default.Register<Session> (() => session);

			WrappedOrders orders = new WrappedOrders ();
			SimpleIoc.Default.Register<WrappedOrders> (() => orders);

			WrappedCars cars = new WrappedCars ();
			SimpleIoc.Default.Register<WrappedCars> (() => cars);

			IClientConnection clientConnection = new ClientConnection ("http://192.168.56.1:8080/commands");
			clientConnection.RegisterCommandHandler (new CmdReturnGetAvailableCarsHandler (cars));
			clientConnection.RegisterCommandHandler (new CmdReturnGetDriversUnfinishedOrdersHandler (orders));
			clientConnection.Start ();
			SimpleIoc.Default.Register<IClientConnection> (() => clientConnection);

			SimpleIoc.Default.Register<GPSPositionSender> ();

			SimpleIoc.Default.Register<LoginViewModel> ();
			SimpleIoc.Default.Register<SelectCarViewModel> ();
			SimpleIoc.Default.Register<OrdersViewModel> ();
			SimpleIoc.Default.Register<OrderDetailsViewModel> ();
			SimpleIoc.Default.Register<LogoutViewModel> ();
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
		public SelectCarViewModel SelectCar
		{
			get
			{
				return ServiceLocator.Current.GetInstance<SelectCarViewModel>();
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public OrderDetailsViewModel OrderDetails
		{
			get {
				return ServiceLocator.Current.GetInstance<OrderDetailsViewModel> ();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public LogoutViewModel Logout
		{
			get {
				return ServiceLocator.Current.GetInstance<LogoutViewModel> ();
			}
		}

	}
}