using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Smartphone.Driver
{
	public partial class OrdersPage : ContentPage
	{
		public OrdersPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.Orders;
		}
	}
}

