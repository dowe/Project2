using System;

using Xamarin.Forms;
using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;

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
			

			MainPage = new LoginPage();
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

		public static LoginPage GetLoginPage()
		{
			return new LoginPage ();
		}
	}
}

