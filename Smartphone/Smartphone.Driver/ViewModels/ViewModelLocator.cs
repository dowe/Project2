using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Common.Communication.Client;
using Smartphone.Driver.Models;
using Smartphone.Driver.Handlers;
using Smartphone.Driver.ViewModels;
using Smartphone.Driver.GPS;
using Smartphone.Driver.NativeServices;
using Xamarin.Forms;

namespace Smartphone.Driver.ViewModels
{
	public class ViewModelLocator
	{
		
		static ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			// Connection
			IClientConnection clientConnection = new ClientConnection ("http://192.168.56.1:8080/commands");
			clientConnection.Start ();
			SimpleIoc.Default.Register<IClientConnection> (() => clientConnection);

			// Model
			Session session = new Session ();
			SimpleIoc.Default.Register<Session> (() => session);
			WrappedOrders orders = new WrappedOrders ();
			SimpleIoc.Default.Register<WrappedOrders> (() => orders);
			WrappedCars cars = new WrappedCars ();
			SimpleIoc.Default.Register<WrappedCars> (() => cars);

			// Native stuff
			IGPSLocator gpsLocator = DependencyService.Get<IGPSLocator> ();
			GPSPositionSender gpsPositionSender = new GPSPositionSender (clientConnection, session, gpsLocator);
			SimpleIoc.Default.Register<GPSPositionSender> (() => gpsPositionSender);
			INotificationController notificationController = DependencyService.Get<INotificationController> ();

			// Connection
			clientConnection.RegisterCommandHandler (new CmdReturnGetAvailableCarsHandler (cars));
			clientConnection.RegisterCommandHandler (new CmdReturnGetDriversUnfinishedOrdersHandler (orders));
			clientConnection.RegisterCommandHandler (new CmdRemindDriverOfOrderHandler (notificationController));
			
			// ViewModels
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