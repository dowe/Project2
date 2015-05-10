using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GalaSoft.MvvmLight.Messaging;

namespace Smartphone.Driver
{
	public partial class OrderDetailsPage : ContentPage
	{
		public OrderDetailsPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.OrderDetails;
		}


		protected override bool OnBackButtonPressed ()
		{
			Messenger.Default.Send<MsgSwitchOrdersPage> (new MsgSwitchOrdersPage ());

			return true;
		}
	}
}

