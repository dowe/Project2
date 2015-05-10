using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartphone.Driver
{
	public partial class OrderDetailsPage : ContentPage
	{
		public OrderDetailsPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.OrderDetails;
		}
	}
}

