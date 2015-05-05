using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartphone.Driver
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.Login;
		}

	}
}

