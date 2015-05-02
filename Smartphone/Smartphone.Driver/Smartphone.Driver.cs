using System;

using Xamarin.Forms;
using Common.Communication.Client;
using Common.Commands;
using Common.DataTransferObjects;

namespace Smartphone.Driver
{
	public class App : Application
	{

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
	}
}

