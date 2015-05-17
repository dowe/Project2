using System;

using Xamarin.Forms;

namespace Kundenapp
{
	public class App : Application
	{
		private static ViewModelLocator _locator;

		public static ViewModelLocator Locator
		{
			get
			{
				return _locator ?? (_locator = new ViewModelLocator());
			}
		}

		public App ()
		{
			// The root page of your application
			AnalysisSelectionVM vm = Locator.AnalysisVM;
			MainPage = new NavigationPage(new Login());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

