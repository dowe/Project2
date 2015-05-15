using System;
using System.Collections.Generic;

using Xamarin.Forms;
using GalaSoft.MvvmLight.Messaging;
using Smartphone.Driver.Messages;

namespace Smartphone.Driver
{
	public partial class OrdersPage : ContentPage
	{
		public OrdersPage ()
		{
			InitializeComponent ();
			BindingContext = App.Locator.Orders;
			Messenger.Default.Register<MsgShowEmergencyDialog>(this, ShowEmergencyDialog);
		}

		private async void ShowEmergencyDialog(MsgShowEmergencyDialog message)
		{
			var answer = await DisplayAlert ("Emergency?", "Are you sure that you want to report an emergency? All unfinished orders will be forwarded to other drivers.", "Yes", "No");

			if (answer)
			{
				if (message.EmergencyConfirmed != null)
				{
					message.EmergencyConfirmed ();
				}
			}
			else
			{
				if (message.EmergencyCanceled != null)
				{
					message.EmergencyCanceled ();
				}
			}
		}

		protected override bool OnBackButtonPressed ()
		{
			Messenger.Default.Send<MsgSwitchLogoutPage> (new MsgSwitchLogoutPage ());

			return true;
		}

	}
}

