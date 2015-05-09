using Xamarin.Forms;
using GalaSoft.MvvmLight.Messaging;

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
			Messenger.Default.Register<MsgSwitchOrdersPage> (this, SwitchOrdersPage);
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

		private void SwitchOrdersPage(MsgSwitchOrdersPage message)
		{
			MainPage = new OrdersPage ();
		}

	}
}

