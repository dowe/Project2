using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartphone.Driver
{
	public partial class LogoutPage : ContentPage
	{
		public LogoutPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.Logout;
		}
	}
}

