using Xamarin.Forms;
using GalaSoft.MvvmLight.Messaging;
using Smartphone.Driver.ViewModels;
using Smartphone.Driver.Messages;

namespace Smartphone.Driver
{
	public class App : Application
	{

		private static ViewModelLocator locator = null;

		public static ViewModelLocator Locator
		{
			get {
				return locator ?? (locator = new ViewModelLocator ());
			}
		}

		public App ()
		{
			RegisterMessengerHandlers ();
			MainPage = new LoginPage ();
		}

		private void RegisterMessengerHandlers()
		{
			Messenger.Default.Register<MsgSwitchSelectCarPage> (this, SwitchSelectCarPage);
			Messenger.Default.Register<MsgSwitchOrdersPage> (this, SwitchOrdersPage);
			Messenger.Default.Register<MsgSwitchOrderDetailsPage> (this, SwitchOrderDetailsPage);
			Messenger.Default.Register<MsgSwitchLogoutPage> (this, SwitchLogoutPage);
			Messenger.Default.Register<MsgSwitchLoginPage> (this, SwitchLoginPage);
		}

		protected override void OnStart ()
		{
			
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

		private void SwitchSelectCarPage(MsgSwitchSelectCarPage message)
		{
			MainPage = new SelectCarPage ();
		}

		private void SwitchOrdersPage(MsgSwitchOrdersPage message)
		{
			MainPage = new OrdersPage ();
		}

		private void SwitchOrderDetailsPage(MsgSwitchOrderDetailsPage message)
		{
			MainPage = new OrderDetailsPage ();
			// Set the model to display.
			Messenger.Default.Send(new MsgSetOrderDetailsModel(message.Order));
		}

		private void SwitchLogoutPage(MsgSwitchLogoutPage message)
		{
			MainPage = new LogoutPage ();
		}

		private void SwitchLoginPage(MsgSwitchLoginPage message)
		{
			MainPage = new LoginPage ();
		}

	}
}

